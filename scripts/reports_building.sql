USE AccessControlSystem
GO

DROP PROCEDURE dbo.ListDomainUsers
GO
DROP PROCEDURE dbo.ListDayLogTargets
GO
DROP FUNCTION Split 
GO
DROP FUNCTION dbo.GetDayLog
GO
DROP FUNCTION dbo.GetDCName
GO

CREATE FUNCTION Split (@InputString VARCHAR(8000), @Delimiter VARCHAR(50))
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

CREATE FUNCTION dbo.GetDCName(@domain VARCHAR(255))
RETURNS VARCHAR(255)
AS 
BEGIN
    DECLARE @dc VARCHAR(500)
    SELECT @dc = COALESCE(@dc + ',', '') + 'dc=' + Item
    FROM Split(@domain, '.')
    RETURN @dc
END
GO

CREATE PROCEDURE dbo.ListDomainUsers(@domain VARCHAR(255))
AS
BEGIN
    DECLARE @dc VARCHAR(500) = dbo.GetDCName(@domain)

    DECLARE @Users TABLE
    (
        Department VARCHAR(255),
        sAMAccountName VARCHAR(255),
        DistinguishedName VARCHAR(255),
        Mail VARCHAR(255),
        Manager VARCHAR(255)
    )

    DECLARE @sql VARCHAR(1000) = 'SELECT Department, sAMAccountName, DistinguishedName, Mail, Manager FROM ''''LDAP://' + @dc + ''''' WHERE objectClass=''''user'''''
    DECLARE @openquery VARCHAR(1000) = 'SELECT Department, sAMAccountName, DistinguishedName, Mail, Manager FROM OPENQUERY(ADDS, ''' + @sql + ''')'
    INSERT INTO @Users (Department, sAMAccountName, DistinguishedName, Mail, Manager)
        EXEC (@openquery)

    SELECT * FROM @Users
END
GO

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
        SELECT GETUTCDATE() 
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
	    WHERE le.CreatedUtc >= i.FromDateTime AND le.CreatedUtc <= i.ToDateTime
    )
    INSERT INTO @DayLog(Id, CreatedUtc, AttemptedHash, UserName, Failed, AccessPointId)
    SELECT *
    FROM DayLog DL
    RETURN;
END
GO


CREATE PROCEDURE dbo.ListDayLogTargets(@domain VARCHAR(255))
AS
BEGIN
    Declare @DomainUsers TABLE
    (
        Department VARCHAR(255),
        sAMAccountName VARCHAR(255),
        Mail VARCHAR(255),
        Manager VARCHAR(255)
    )
    INSERT INTO @DomainUsers(Department, sAMAccountName, Mail, Manager)
        EXEC dbo.ListDomainUsers @domain

    SELECT * 
    FROM dbo.GetDayLog(-1) DL
        INNER JOIN dbo.AccessPoint AP ON AP.AccessPointId = DL.AccessPointId
END
GO
   

DECLARE @domain VARCHAR(255) = 'evriqum.ru'

    SELECT DISTINCT 
        Department, 
        ManagedBy AS DomainSpecificName, 
        SUBSTRING(ManagedBy, 0, CHARINDEX('\', ManagedBy)) AS Domain,
        SUBSTRING(ManagedBy, CHARINDEX('\', ManagedBy) + 1, 255) AS sAMAccountName
    FROM dbo.GetDayLog(0) DL
    INNER JOIN (SELECT * FROM dbo.AccessPoint) AP ON AP.AccessPointId = DL.AccessPointId
    CROSS APPLY (SELECT * FROM dbo.Split(AP.ManagedBy, '\')) APA


    EXEC dbo.ListDomainUsers @domain