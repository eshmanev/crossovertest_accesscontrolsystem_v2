. .\common.ps1

function ModifySmtpSettings($file, $server, $port, $ssl, $user, $password, $senderName, $senderAddress)
{
  $config = LoadConfig -file $file
  $node = $config.SelectSingleNode('configuration/notification/smtp')
  $node.Attributes["host"].Value = $server
  $node.Attributes["port"].Value = $port
  $node.Attributes["useSsl"].Value = $ssl
  $node.Attributes["user"].Value = $user
  $node.Attributes["password"].Value = $password
  $node.Attributes["senderName"].Value = $senderName
  $node.Attributes["fromAddress"].Value = $senderAddress
  $config.Save($file)
}

function ModifyTwilio($file, $sid, $token, $number)
{
  $config = LoadConfig -file $file
  $node = $config.SelectSingleNode('configuration/notification/twilio')
  $node.Attributes["accountSid"].Value = $sid
  $node.Attributes["authToken"].Value = $token
  $node.Attributes["fromNumber"].Value = $number
  $config.Save($file)
}

# LDAP parameters
$ldapAddress = AskParameter -message "Enter LDAP directory server address" -default "LDAP://192.168.1.210"
$ldapUserName = AskParameter -message "Enter LDAP directory username" -default "ldapservice"
$ldapPassword = AskParameter -message "Enter LDAP directory password" -default "Test123"
# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter RabbitMQ server address" -default "rabbitmq://192.168.1.230"
$rabbitUserName = AskParameter -message "Enter RabbitMQ username" -default "evgeny"
$rabbitPassword = AskParameter -message "Enter RabbitMQ password" -default "Test123"
# Database parameters
$connectionString = AskParameter -message "Enter database connection string" -default "Data Source=SQL\SQLSERVER;Initial Catalog=AccessControlSystem;Integrated Security=false;User Id=accuser;Password=Test123"
#smtp
$smtpHost = AskParameter -message "Enter SMTP host" -default "smtp.gmail.com"
$smtpPort = AskParameter -message "Enter SMTP port" -default "587"
$smtpSsl = AskParameter -message "Enable SSL? [true/false]" -default "true"
$smtpUser = AskParameter -message "Enter SMTP user name" -default "test30259@gmail.com"
$smtpPassword = AskParameter -message "Enter SMTP password" -default "test30259test30259"
$smtpSenderName = AskParameter -message "Enter sender's display name" -default "Access Control System"
$smtpSenderAddress = AskParameter -message "Enter sender's email" -default "test30259@gmail.com"
#twilio
$twilioSid = AskParameter -message "Enter Twilio identifier" -default "AC60671f39f9c1edd419022c8f0f9efe9e"
$twilioToken = AskParameter -message "Enter Twilio AUTH token" -default "f62e1374032b4b2838d07f085b678d15"
$twilioNumber = AskParameter -message "Enter Twilio phone number" -default "+13095884640"

# Do work
Unzip "AccessControl.Service.LDAP.zip" ".\AccessControl.Service.LDAP"
ModifyConfigLDAP -file ".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe.config" -url $ldapAddress -user $ldapUserName -password $ldapPassword
ModifyConfigRabbit -file ".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
InstallAndRun(".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe")

Unzip "AccessControl.Service.Notifications.zip" ".\AccessControl.Service.Notifications"
ModifyConfigRabbit -file ".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
ModifySmtpSettings -file ".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe.config" -server $smtpHost -port $smtpPort -ssl $smtpSsl -user $smtpUser -password $smtpPassword -senderName $smtpSenderName -senderAddress $smtpSenderAddress
ModifyTwilio -file ".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe.config" -sid $twilioSid -token $twilioToken -number $twilioNumber
InstallAndRun(".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe")

Unzip "AccessControl.Service.AccessPoint.zip" ".\AccessControl.Service.AccessPoint"
ModifyConfigRabbit -file ".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe.config" -url $rabbitAddress -user $rabbitUserName -password $rabbitPassword
ModifyConnectionString -file ".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe.config" -value $connectionString
InstallAndRun(".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe")

