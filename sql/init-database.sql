USE [master]
GO

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Payments')
 CREATE DATABASE [Payments]


If not Exists (select loginname from master.dbo.syslogins 
	where name = 'payments-user')

begin

CREATE LOGIN [payments-user] WITH PASSWORD=N'1qaz@WSX', 
	DEFAULT_DATABASE=[Payments], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF

end

GO

use [Payments]

IF NOT EXISTS (SELECT name 
                FROM [sys].[database_principals]
                WHERE [type] = 'S' AND name = N'payments-user')

begin

CREATE USER [payments-user] FOR LOGIN [payments-user]
	WITH DEFAULT_SCHEMA = [dbo]

EXEC sp_addrolemember N'db_owner', N'payments-user'


end


