. .\common.ps1

function ModifyConfigAppSettings($file, $user, $password)
{
  $config = LoadConfig -file $file
  $node = $config.SelectSingleNode("configuration/appSettings/add[@key='LDAPUserName']")
  $node.Attributes["value"].Value = $user
  $node = $config.SelectSingleNode("configuration/appSettings/add[@key='LDAPPassword']")
  $node.Attributes["value"].Value = $password
  $config.Save($file)
}

# LDAP parameters
$userName = AskParameter -message "Enter client service user name" -default "evriqum\client1"
$password = AskParameter -message "Enter client service password" -default "Test123"

# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter RabbitMQ server address" -default "rabbitmq://192.168.1.220"
$rabbitUserName = AskParameter -message "Enter RabbitMQ username" -default "evgeny"
$rabbitPassword = AskParameter -message "Enter RabbitMQ password" -default "Test123"

# Do work
Unzip "AccessControl.Client.zip" ".\AccessControl.Client"
ModifyConfigRabbit -file ".\AccessControl.Client\AccessControl.Client.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
ModifyConfigAppSettings -file ".\AccessControl.Client\AccessControl.Client.exe.config" -user $userName -password $password
InstallAndRun(".\AccessControl.Client\AccessControl.Client.exe")

Unzip "AccessSimulator.zip" ".\AccessSimulator"
