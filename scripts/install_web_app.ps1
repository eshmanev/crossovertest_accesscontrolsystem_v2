. .\common.ps1

# LDAP parameters
# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter RabbitMQ server address" -default "rabbitmq://192.168.1.230"
$rabbitUserName = AskParameter -message "Enter RabbitMQ username" -default "evgeny"
$rabbitPassword = AskParameter -message "Enter RabbitMQ password" -default "Test123"

# Do work
Unzip "AccessControl.Web.zip" ".\AccessControl.Web"
ModifyConfigRabbit -file ".\AccessControl.Web\web.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword -nodePath "configuration/rabbitMq"
DeployWebsite -pool "AccessControl.Web" -dotNetVersion "v4.0" -appName "AccessControl.Web" -path ".\AccessControl.Web" -port 8967
