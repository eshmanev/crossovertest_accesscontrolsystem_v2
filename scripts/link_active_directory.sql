DECLARE @userName varchar(30) = N'evriqum\Administrator'
DECLARE @password varchar(30) = N'Nata308son'


EXEC master.dbo.sp_addlinkedserver @server = N'ADDS'
, @srvproduct=N'Active Directory Service Interfaces'
, @provider=N'ADSDSOObject'
, @datasrc=N'adsdatasource'

EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'ADDS'
,@useself=N'False'
,@locallogin=NULL
,@rmtuser= @userName
,@rmtpassword=@password