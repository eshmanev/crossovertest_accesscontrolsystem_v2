. .\common.ps1

StopAndUninstall(".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe")
StopAndUninstall(".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe")
StopAndUninstall(".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe")
UninstallWebsite -pool "AccessControl.Web" -appName "AccessControl.Web" -path ".\AccessControl.Web"
