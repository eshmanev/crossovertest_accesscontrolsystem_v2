. .\common.ps1

function ModifyConfigAppSettings($file, $user, $password)
{
  $config = LoadConfig -file $file

  $node = $config.SelectSingleNode("configuration/appSettings/add[@key='LDAPUserName']")
  $node.Attributes["value"].Value = $user

  $node = $config.SelectSingleNode("configuration/appSettings/add[@key='LDAPPassword']")
  $node.Attributes["value"].Value = $user

  $config.Save($file)
}

# LDAP parameters
$userName = AskParameter -message "Enter the client service user name" -error "Client service user name is required"
$password = AskParameter -message "Enter the client service password" -error "Client service password is required"

# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter the RabbitMQ server address (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ server address is required"
$rabbitUserName = AskParameter -message "Enter the RabbitMQ username (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ username is required"
$rabbitPassword = AskParameter -message "Enter the RabbitMQ password (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ password is required"

# Do work
Unzip "AccessControl.Client.zip" ".\AccessControl.Client"
ModifyConfigRabbit -file ".\AccessControl.Client\AccessControl.Client.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
ModifyConfigAppSettings -file ".\AccessControl.Client\AccessControl.Client.exe.config" -user $userName -password $password
InstallAndRun(".\AccessControl.Client\AccessControl.Client.exe")

Unzip "AccessSimulator.zip" ".\AccessSimulator"
