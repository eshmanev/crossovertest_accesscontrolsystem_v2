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

Unzip "AccessControl.Service.LDAP.zip" ".\AccessControl.Service.LDAP"
InstallAndRun(".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe")

Unzip "AccessControl.Service.Notifications.zip" ".\AccessControl.Service.Notifications"
InstallAndRun(".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe")

Unzip "AccessControl.Service.AccessPoint.zip" ".\AccessControl.Service.AccessPoint"
InstallAndRun(".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe")

Unzip "AccessControl.Web.zip" ".\AccessControl.Web"
DeployWebsite -pool "AccessControl.Web" -dotNetVersion "v4.0" -appName "AccessControl.Web" -path ".\AccessControl.Web" -port 8967
