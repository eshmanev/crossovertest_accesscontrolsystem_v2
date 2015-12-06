USE AccessControlSystem
GO
DECLARE @AD_UserName varchar(30) = N'<domain>\<user>'
DECLARE @AD_Password varchar(30) = N'<password>'

-- Links Active Directory Server
EXEC master.dbo.sp_addlinkedserver 
    @server = N'ADDS',
    @srvproduct=N'Active Directory Service Interfaces', 
    @provider=N'ADSDSOObject', 
    @datasrc=N'adsdatasource'

EXEC master.dbo.sp_addlinkedsrvlogin 
    @rmtsrvname=N'ADDS',
    @useself=N'False',
    @locallogin=NULL,
    @rmtuser= @AD_UserName,
    @rmtpassword=@AD_Password
GO


--DROP PROCEDURE dbo.ListDomainUsers
--GO
--DROP PROCEDURE dbo.ListDayLogTargets
--GO
--DROP PROCEDURE dbo.CreateDepartmentReport
--GO
--DROP FUNCTION Split 
--GO
--DROP FUNCTION dbo.GetDayLog
--GO
--DROP FUNCTION dbo.GetDCName
--GO
--DROP FUNCTION dbo.ToLocalTime
--GO


-- Converts UTC time to local time
CREATE FUNCTION dbo.ToLocalTime(@datetime DATETIME)
RETURNS DATETIME
BEGIN
    RETURN CONVERT(DATETIME, SWITCHOFFSET(CONVERT(DATETIMEOFFSET, @datetime), DATENAME(TzOffset, SYSDATETIMEOFFSET()))) 
END
GO

-- Splits the given string and returns a table with substrings
CREATE FUNCTION dbo.Split (@InputString VARCHAR(8000), @Delimiter VARCHAR(50))
RETURNS @Items TABLE(Item VARCHAR(8000))
AS
BEGIN
    IF @Delimiter = ' '
    BEGIN
        SET @Delimiter = ','
        SET @InputString = REPLACE(@InputString, ' ', @Delimiter)
    END

    IF (@Delimiter IS NULL OR @Delimiter = '')
        SET @Delimiter = ','

    DECLARE @Item VARCHAR(8000)
    DECLARE @ItemList VARCHAR(8000)
    DECLARE @DelimIndex INT

    SET @ItemList = @InputString
    SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
    WHILE (@DelimIndex != 0)
    BEGIN
        SET @Item = SUBSTRING(@ItemList, 0, @DelimIndex)
        INSERT INTO @Items VALUES (@Item)

        -- Set @ItemList = @ItemList minus one less item
        SET @ItemList = SUBSTRING(@ItemList, @DelimIndex+1, LEN(@ItemList)-@DelimIndex)
        SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
    END

    -- At least one delimiter was encountered in @InputString
    IF @Item IS NOT NULL
    BEGIN
        SET @Item = @ItemList
        INSERT INTO @Items VALUES (@Item)
    END
    ELSE INSERT INTO @Items VALUES (@InputString)
    RETURN
END
GO

-- Converts the given name to "DC=xxx,DC=yyy"
CREATE FUNCTION dbo.GetDCName(@domainName VARCHAR(255))
RETURNS VARCHAR(255)
AS 
BEGIN
    DECLARE @index INT = CHARINDEX('DC=', @domainName)
    DECLARE @dc VARCHAR(500)
    IF @index != 0
    BEGIN
        -- Distinguished name => DC=domain,DC=com
        SELECT @dc = SUBSTRING(@domainName, @index, LEN(@domainName))
    END
    ELSE
    BEGIN
        -- remove user name
        SELECT @index = CHARINDEX('\', @domainName)
        IF @index != 0
            SELECT @domainName = SUBSTRING(@domainName, 0, @index)

        -- domain.com -> DC=domain,DC=com
        SELECT @dc = COALESCE(@dc + ',', '') + 'DC=' + Item
        FROM Split(@domainName, '.')
    END
    RETURN @dc
END
GO

-- Lists domain users
CREATE PROCEDURE dbo.ListDomainUsers(@domain VARCHAR(255))
AS
BEGIN
    DECLARE @dc VARCHAR(500) = dbo.GetDCName(@domain)
    DECLARE @sql VARCHAR(1000) = 'SELECT Department, sAMAccountName, DistinguishedName, Mail, Manager, CN FROM ''''LDAP://' + @dc + ''''' WHERE objectClass=''''user'''''
    DECLARE @openquery VARCHAR(1000) = 'SELECT * FROM OPENQUERY(ADDS, ''' + @sql + ''')'
    EXEC (@openquery)
END
GO

-- Get a day log. @dayOffset - offset relative to current day. Positive value means future, negative value means past, zero mean the present day. 
CREATE FUNCTION dbo.GetDayLog(@dayOffset INT)
RETURNS @DayLog TABLE 
(
    Id INT,
    CreatedUtc DATETIME,
    AttemptedHash VARCHAR(255),
    UserName VARCHAR(255),
    Failed BIT,
    AccessPointId UNIQUEIDENTIFIER
)
AS
BEGIN
    WITH CurrentDate(Value) AS 
    ( 
        SELECT GETDATE()
    ),
    CurrentDateParts([Year], [Month], [Day], [Hour], [Minute], [Second]) AS
    (
	    SELECT DATEPART(YEAR, Value), DATEPART(MONTH, Value), DATEPART(DAY, Value), DATEPART(HOUR, VALUE), DATEPART(MINUTE, VALUE), DATEPART(SECOND, VALUE)
	    FROM CurrentDate
    ),
    Interval(FromDateTime, ToDateTime) AS
    (
	    SELECT DATETIMEFROMPARTS([Year], [Month], [Day] + @dayOffset, 0, 0, 0, 0), DATETIMEFROMPARTS([Year], [Month], [Day] + @dayOffset, 23, 59, 59, 999)
	    FROM CurrentDateParts
    ),
    DayLog AS
    (
	    SELECT le.*
	    FROM dbo.LogEntry le, Interval i
	    WHERE dbo.ToLocalTime(le.CreatedUtc) >= i.FromDateTime AND dbo.ToLocalTime(le.CreatedUtc) <= i.ToDateTime
    )
    INSERT INTO @DayLog(Id, CreatedUtc, AttemptedHash, UserName, Failed, AccessPointId)
    SELECT *
    FROM DayLog DL
    RETURN;
END
GO

-- Returns a list of department managers and departments with day attendance
-- NOTE: This procedure will be used to schedule dynamic reports
CREATE PROCEDURE dbo.ListDayLogTargets(@domain VARCHAR(255), @dayOffset INT)
AS
BEGIN
    Declare @DomainUsers TABLE
    (
        Department VARCHAR(255),
        sAMAccountName VARCHAR(255),
        DistinguishedName VARCHAR(255),
        Mail VARCHAR(255),
        CN VARCHAR(255)
    )
    DECLARE @Attendances TABLE
    (
        Department VARCHAR(255),
        Domain VARCHAR(255),
        ManagerName VARCHAR(255),
        DomainManagerName VARCHAR(255)
    )

    DECLARE @dc VARCHAR(500) = dbo.GetDCName(@domain)
    DECLARE @sql VARCHAR(1000) = 'SELECT Department, sAMAccountName, DistinguishedName, Mail, CN FROM ''''LDAP://' + @dc + ''''' WHERE objectClass=''''user'''''
    DECLARE @openquery VARCHAR(1000) = 'SELECT Department, sAMAccountName, DistinguishedName, Mail, CN FROM OPENQUERY(ADDS, ''' + @sql + ''')'
    INSERT INTO @DomainUsers (Department, sAMAccountName, DistinguishedName, Mail, CN)
        EXEC (@openquery)

    INSERT INTO @Attendances(Department, Domain, ManagerName, DomainManagerName)
        SELECT DISTINCT 
            Department, 
            dbo.GetDCName(SUBSTRING(ManagedBy, 0, CHARINDEX('\', ManagedBy))) AS Domain,
            SUBSTRING(ManagedBy, CHARINDEX('\', ManagedBy) + 1, 255) AS sAMAccountName,
            AP.ManagedBy
        FROM dbo.GetDayLog(@dayOffset) DL
        INNER JOIN (SELECT * FROM dbo.AccessPoint) AP ON AP.AccessPointId = DL.AccessPointId

    DECLARE @created DATETIME = GETDATE()
    SELECT
        @created as Created,
        DU.Department + '_' + CONVERT(VARCHAR, @created, 105) as FileName,
        DU.Department AS Department,
        DU.CN AS Manager,
        DU.Mail AS ManagerMail,
        A.DomainManagerName
    FROM @DomainUsers DU
        INNER JOIN @Attendances A 
            ON  A.Domain = dbo.GetDCName(DU.DistinguishedName)
            AND A.ManagerName = DU.sAMAccountName
END
GO
   
-- Creates the report
CREATE PROCEDURE dbo.CreateDepartmentReport(@domainManagerName VARCHAR(255), @dayOffset INT)
AS
BEGIN
    DECLARE @DomainUsers TABLE
    (
        Department VARCHAR(255),
        sAMAccountName VARCHAR(255),
        DistinguishedName VARCHAR(255),
        Mail VARCHAR(255),
        CN VARCHAR(255),
        DomainUserName VARCHAR(255)
    )
    
    DECLARE @domainName VARCHAR(255) = SUBSTRING(@domainManagerName, 0, CHARINDEX('\', @domainManagerName))
    DECLARE @dc VARCHAR(255) = dbo.GetDCName(@domainName)
    DECLARE @sql VARCHAR(1000) = 'SELECT sAMAccountName, DistinguishedName, Mail, CN FROM ''''LDAP://' + @dc + ''''' WHERE objectClass=''''user'''''
    DECLARE @openquery VARCHAR(1000) = 'SELECT sAMAccountName, DistinguishedName, Mail, CN FROM OPENQUERY(ADDS, ''' + @sql + ''')'
    INSERT INTO @DomainUsers (sAMAccountName, DistinguishedName, Mail, CN)
        EXEC (@openquery)

    UPDATE @DomainUsers SET DomainUserName = @domainName + '\' + sAMAccountName
    FROM @DomainUsers

    SELECT
        DL.CreatedUtc,
        DL.AttemptedHash,
        DL.Failed,
        AP.Department,
        AP.Name AS AccessPointName,
        DU.CN AS Employee,
        DU.Mail AS EmployeeMail
    FROM dbo.AccessPoint AP
        INNER JOIN dbo.GetDayLog(@dayOffset) DL ON DL.AccessPointId = AP.AccessPointId
        INNER JOIN @DomainUsers DU ON DU.DomainUserName = DL.UserName
    WHERE AP.ManagedBy = @domainManagerName
END
GO