. .\common.ps1

# LDAP parameters
$ldapAddress = AskParameter -message "Enter the LDAP directory server address (Example: LDAP://192.168.1.201)" -error "LDAP directory address is required"
$ldapUserName = AskParameter -message "Enter the LDAP directory username" -error "LDAP username is required"
$ldapPassword = AskParameter -message "Enter the LDAP directory password" -error "LDAP password is required"
# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter the RabbitMQ server address (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ server address is required"
$rabbitUserName = AskParameter -message "Enter the RabbitMQ username" -error "RabbitMQ username is required"
$rabbitPassword = AskParameter -message "Enter the RabbitMQ password" -error "RabbitMQ password is required"
# Database parameters
$connectionString = AskParameter -message "Enter the database connection string (Example: 'Data Source=.\SQLEXPRESS;Initial Catalog=AccessControlSystem;Integrated Security=True')" -error "Connection string is required"

# Do work
Unzip "AccessControl.Service.LDAP.zip" ".\AccessControl.Service.LDAP"
ModifyConfigLDAP -file ".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe.config" -url $ldapAddress -user $ldapUserName -password $ldapPassword
ModifyConfigRabbit -file ".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
InstallAndRun(".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe")

Unzip "AccessControl.Service.Notifications.zip" ".\AccessControl.Service.Notifications"
ModifyConfigRabbit -file ".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
InstallAndRun(".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe")

Unzip "AccessControl.Service.AccessPoint.zip" ".\AccessControl.Service.AccessPoint"
ModifyConfigRabbit -file ".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
ModifyConnectionString -file ".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe.config" -value $connectionString
InstallAndRun(".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe")

Unzip "AccessControl.Web.zip" ".\AccessControl.Web"
ModifyConfigRabbit -file ".\AccessControl.Web\web.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword -nodePath "configuration/rabbitMq"
DeployWebsite -pool "AccessControl.Web" -dotNetVersion "v4.0" -appName "AccessControl.Web" -path ".\AccessControl.Web" -port 8967
