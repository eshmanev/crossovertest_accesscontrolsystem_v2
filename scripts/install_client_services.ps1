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

function AllowWcfUrl($url)
{
  $cmd = "netsh http delete urlacl " + $url
  Invoke-Expression -command $cmd

  $cmd = "netsh http add urlacl " + $url + " user=""Local Service"""
  Invoke-Expression -command $cmd
}

AllowWcfUrl -url http://+:9981/AccessCheckService
if ($LASTEXITCODE -ne 0)
{
    Exit;
}

# LDAP parameters
$userName = AskParameter -message "Enter client service user name" -default "evriqum\client1"
$password = AskParameter -message "Enter client service password" -default "Test123"

# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter RabbitMQ server address" -default "rabbitmq://127.0.0.1"
$rabbitUserName = AskParameter -message "Enter RabbitMQ username" -default "evgeny"
$rabbitPassword = AskParameter -message "Enter RabbitMQ password" -default "Test123"

# Do work
Unzip "AccessControl.Client.zip" ".\AccessControl.Client"
ModifyConfigRabbit -file ".\AccessControl.Client\AccessControl.Client.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
ModifyConfigAppSettings -file ".\AccessControl.Client\AccessControl.Client.exe.config" -user $userName -password $password
InstallAndRun(".\AccessControl.Client\AccessControl.Client.exe")

Unzip "AccessSimulator.zip" ".\AccessSimulator"
