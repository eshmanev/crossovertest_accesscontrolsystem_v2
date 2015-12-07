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

function UninstallWebsite($pool, $dotNetVersion, $appName, $path, $port) {
  #check if the site exists
  try { $appExists = Get-Website -Name $appName }
  catch {}
  if ($appExists)
  {
    Remove-Website -Name $appName
    Write-Host "Website $appName was successfully removed"
  }

  #check if the app pool exists
  try { $poolExists = Get-WebAppPoolState -Name $pool }
  catch {}
  if ($poolExists)
  {
    Remove-WebAppPool -Name $pool
    Write-Host "AppPool $pool was successfully removed"
  }
}

function InstallAndRun($servicePath)
{
  try
  {
    $fullPath = [System.IO.Path]::GetFullPath($servicePath)
    & $fullPath "install" "--localservice" "--autostart"
    & $fullPath "start"
  }
  catch
  {
    Write-Host "[ERROR]`t Unable to start service $servicePath. Script will stop!"
    Exit 1
  }
}

function StopAndUninstall($servicePath)
{
  try
  {
    $fullPath = [System.IO.Path]::GetFullPath($servicePath)
    & $fullPath "uninstall"
  }
  catch
  {
    Write-Host "[ERROR]`t Unable to stop and uninstall service $servicePath."
  }
}

function AskParameter($message, $default)
{
  if ($default)
  {
    $message = $message + " (default: " + $default + " )"
  }

  $value = Read-Host -Prompt $message
  if (!$value)
  {
    if (!$default)
    {
        Write-Host "[ERROR]`t Value is required"
        Exit 1
    }
    else
    {
        $value = $default
    }
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
  $node = $config.SelectSingleNode('configuration/ldap/directories/add')

  # capitalize protocol name; otherwise it may lead ComException
  if ($url.ToLower().StartsWith("ldap://"))
  {
      $url = $url.Remove(0, 4);
      $url = "LDAP" + $url;
  }

  $node.Attributes["url"].Value = $url
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
