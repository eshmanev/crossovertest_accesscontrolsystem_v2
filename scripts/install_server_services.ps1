Try
{
  Add-Type -AssemblyName System.IO.Compression.FileSystem
}
Catch
{
  Write-Host "[ERROR]`t System.IO.Compression.FileSystem couldn't be loaded. Script will stop!"
  Exit 1
}

Try
{
  #Import-Module ServerManager
  #Add-WindowsFeature Web-Scripting-Tools
  Import-Module WebAdministration
}
Catch
{
  Write-Host "[ERROR]`t WebAdministration module couldn't be loaded. Script will stop!"
  Exit 1
}


function Unzip
{
    param([string]$zipfile, [string]$outpath)
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

function DeployWebsite($pool, $dotNetVersion, $appName, $path, $port) {
  #cd IIS:\AppPools\

  try { $poolExists = Get-WebAppPoolState -Name $pool }
  catch {}

  #check if the app pool exists
  if ($poolExists)
  {
    Write-Host "[ERROR]`t $pool pool already exists. Script will stop!"
    Exit 1
  }

  #create the app pool
  New-WebAppPool -Name $pool
  Set-ItemProperty IIS:\AppPools\$pool managedRuntimeVersion $dotNetVersion

  #navigate to the sites root
  cd IIS:\Sites\

  #check if the site exists
  try { $appExists = Get-Website -Name $appName }
  catch {}

  if ($app)
  {
    Write-Host "[ERROR]`t $appName application already exists. Script will stop!"
    Exit 1
  }

  #create the site
  $fullPath = [System.IO.Path]::GetFullPath($path)
  New-Website -Name $appName -PhysicalPath $fullPath -ApplicationPool $pool
  Set-WebBinding -Name $appName -BindingInformation "*:80:" -PropertyName "Port" -Value "$port"
  Start-Website -Name $appName
}

function InstallAndRun($servicePath)
{
  try
  {
    $fullPath = [System.IO.Path]::GetFullPath($servicePath)
    & $fullPath "install" "--networkservice"
    & $fullPath "start"
  }
  catch
  {
    Write-Host "[ERROR]`t Unable to start service $servicePath. Script will stop!"
    Exit 1
  }
}

function AskParameter($message, $error)
{
  $value = Read-Host -Prompt $message
  if (!$value)
  {
    Write-Host "[ERROR]`t " + $error
    Exit 1
  }
  return $value
}

function LoadConfig($file)
{
  $path = [System.IO.Path]::GetFullPath($file)
  $config = New-Object System.Xml.XmlDocument
  $config.Load($path)
  return $config
}

function ModifyConfigRabbit($file, $url, $user, $password, $nodePath)
{
  if (!$nodePath)
  {
    $nodePath = "configuration/service/rabbitMq";
  }

  $config = LoadConfig -file $file
  $node = $config.SelectSingleNode($nodePath)
  $node.Attributes["url"].Value = $url
  $node.Attributes["userName"].Value = $user
  $node.Attributes["password"].Value = $password
  $config.Save($file)
}

function ModifyConfigLDAP($file, $url, $user, $password)
{
  $config = LoadConfig -file $file
  $node = $config.SelectSingleNode('configuration/ldap')
  $node.Attributes["ldapPath"].Value = $url
  $node.Attributes["userName"].Value = $user
  $node.Attributes["password"].Value = $password
  $config.Save($file)
}

function ModifyConnectionString($file, $value)
{
  $config = LoadConfig -file $file
  $node = $config.SelectSingleNode("configuration/connectionStrings/add[@name = 'AccessControlSystem']")
  $node.Attributes["connectionString"].Value = $value
  $config.Save($file)
}

# LDAP parameters
$ldapAddress = AskParameter -message "Enter the LDAP directory server address (Example: LDAP://192.168.1.201)" -error "LDAP directory address is required"
$ldapUserName = AskParameter -message "Enter the LDAP directory username" -error "LDAP username is required"
$ldapPassword = AskParameter -message "Enter the LDAP directory password" -error "LDAP password is required"
# RabbitMQ parameters
$rabbitAddress = AskParameter -message "Enter the RabbitMQ server address (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ server address is required"
$rabbitUserName = AskParameter -message "Enter the RabbitMQ username (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ username is required"
$rabbitPassword = AskParameter -message "Enter the RabbitMQ password (Example: rabbitmq://192.168.1.201)" -error "RabbitMQ password is required"
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
