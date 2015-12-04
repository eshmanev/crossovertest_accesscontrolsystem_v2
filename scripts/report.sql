
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
	SELECT DATETIMEFROMPARTS([Year], [Month], [Day], 0, 0, 0, 0), DATETIMEFROMPARTS([Year], [Month], [Day], 23, 59, 59, 999)
	FROM CurrentDateParts
),
DayLog AS
(
	SELECT le.*
	FROM dbo.LogEntry le, Interval i
	WHERE le.CreatedUtc >= i.FromDateTime AND le.CreatedUtc <= i.ToDateTime
),
Users AS
(
	SELECT *
	FROM OPENQUERY(ACTIVE_DIRECTORY,'SELECT Department, Manager, DistinguishedName, CN, sAMAccountName, Mail FROM ''LDAP://dc=evriqum,dc=ru'' WHERE objectClass=''user''')
)
SELECT 
	l.CreatedUtc,
	l.Failed,
	l.AttemptedHash,
	ap.Name AS AccessPoint,
	u.Department AS Department, 
	m.CN AS ManagerName,
	m.Mail AS ManagerMail,
	u.CN AS EmployeeName,
	u.Mail AS EmployeeMail
FROM DayLog l
	LEFT JOIN AccessPoint ap on ap.AccessPointId = l.AccessPointId
	LEFT JOIN Users u ON u.sAMAccountName = l.UserName
	LEFT JOIN Users m ON m.DistinguishedName = u.Manager