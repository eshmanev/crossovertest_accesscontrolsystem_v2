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

StopAndUninstall(".\AccessControl.Service.LDAP\AccessControl.Service.LDAP.exe")
StopAndUninstall(".\AccessControl.Service.Notifications\AccessControl.Service.Notifications.exe")
StopAndUninstall(".\AccessControl.Service.AccessPoint\AccessControl.Service.AccessPoint.exe")
UninstallWebsite -pool "AccessControl.Web" -appName "AccessControl.Web" -path ".\AccessControl.Web"
