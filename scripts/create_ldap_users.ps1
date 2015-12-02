###########################################################
# AUTHOR  : Marius / Hican - http://www.hican.nl - @hicannl 
# DATE    : 26-04-2012 
# EDIT    : 07-08-2014
# COMMENT : This script creates new Active Directory users,
#           including different kind of properties, based
#           on an input_create_ad_users.csv.
# VERSION : 1.3
###########################################################

# CHANGELOG
# Version 1.2: 15-04-2014 - Changed the code for better
# - Added better Error Handling and Reporting.
# - Changed input file with more logical headers.
# - Added functionality for account Enabled,
#   PasswordNeverExpires, ProfilePath, ScriptPath,
#   HomeDirectory and HomeDrive
# - Added the option to move every user to a different OU.
# Version 1.3: 08-07-2014
# - Added functionality for ProxyAddresses

# ERROR REPORTING ALL
Set-StrictMode -Version latest

#----------------------------------------------------------
# LOAD ASSEMBLIES AND MODULES
#----------------------------------------------------------
Try
{
  Import-Module ActiveDirectory -ErrorAction Stop
}
Catch
{
  Write-Host "[ERROR]`t ActiveDirectory Module couldn't be loaded. Script will stop!"
  Exit 1
}

#----------------------------------------------------------
#STATIC VARIABLES
#----------------------------------------------------------
$path     = Split-Path -parent $MyInvocation.MyCommand.Definition
$newpath  = $path + "\import_create_ad_users.csv"
$log      = $path + "\create_ad_users.log"
$date     = Get-Date
$addn     = (Get-ADDomain).DistinguishedName
$dnsroot  = (Get-ADDomain).DNSRoot
$i        = 1

#----------------------------------------------------------
#START FUNCTIONS
#----------------------------------------------------------
Function Start-Commands
{
  Create-Users
}

Function Create-Users
{
  "Processing started (on " + $date + "): " | Out-File $log -append
  "--------------------------------------------" | Out-File $log -append
  Import-CSV $newpath | ForEach-Object {
    If (($_.Implement.ToLower()) -eq "yes")
    {
        # Set the target OU
        $location = $_.TargetOU + ",$($addn)"

        # Set the Enabled and PasswordNeverExpires properties
        If (($_.Enabled.ToLower()) -eq "true") { $enabled = $True } Else { $enabled = $False }
        If (($_.PasswordNeverExpires.ToLower()) -eq "true") { $expires = $True } Else { $expires = $False }

        # Create sAMAccountName according to this 'naming convention':
        # <FirstLetterInitials><FirstFourLettersLastName> for example
        # htehp
        $sam = $_.UserName
        Try   { $exists = Get-ADUser -LDAPFilter "(sAMAccountName=$sam)" }
        Catch { }
        If(!$exists)
        {
          # Set all variables according to the table names in the Excel 
          # sheet / import CSV. The names can differ in every project, but 
          # if the names change, make sure to change it below as well.
          $setpass = ConvertTo-SecureString -AsPlainText $_.Password -force

          Try
          {
            Write-Host "[INFO]`t Creating user : $($sam)"
            "[INFO]`t Creating user : $($sam)" | Out-File $log -append
            if($_.Manager)
            {
                 New-ADUser $sam -GivenName $_.FirstName `
                -Surname $_.LastName -DisplayName ($_.FirstName + " " + $_.LastName) `
                -Description $_.Description -EmailAddress $_.Mail `
                -UserPrincipalName ($sam.ToLower() + "@" + $dnsroot) `
                -Department $_.Department `
                -OfficePhone $_.Phone -AccountPassword $setpass -Manager $_.Manager `
                -Enabled $enabled -PasswordNeverExpires $expires
            }
            else
            {
                 New-ADUser $sam -GivenName $_.FirstName `
                -Surname $_.LastName -DisplayName ($_.FirstName + " " + $_.LastName) `
                -Description $_.Description -EmailAddress $_.Mail `
                -UserPrincipalName ($sam.ToLower() + "@" + $dnsroot) `
                -Department $_.Department `
                -OfficePhone $_.Phone -AccountPassword $setpass `
                -Enabled $enabled -PasswordNeverExpires $expires
            }
           
            Write-Host "[INFO]`t Created new user : $($sam)"
            "[INFO]`t Created new user : $($sam)" | Out-File $log -append
     
            $dn = (Get-ADUser $sam).DistinguishedName
            
            # Create ogranizational unit
            If (![adsi]::Exists("LDAP://$($location)"))
            {
            
              $index = $_.TargetOU.IndexOf("=");
              $newOuName = $_.TargetOU.Remove(0, $index + 1);
              New-ADOrganizationalUnit -Name $newOuName -DisplayName $newOuName -Path "$($addn)"
            }

            # Move the user to the OU ($location) you set above. If you don't
            # want to move the user(s) and just create them in the global Users
            # OU, comment the string below
            Move-ADObject -Identity $dn -TargetPath $location
            Write-Host "[INFO]`t User $sam moved to target OU : $($location)"
            "[INFO]`t User $sam moved to target OU : $($location)" | Out-File $log -append
       
            # Add the user to the user group.
            Try   { $exists = Get-ADGroup $_.UserGroup }
            Catch { }
            If(!$exists)
            {
               if ($_.Manager)
                {
                    New-ADGroup -Name $_.UserGroup -GroupScope DomainLocal -ManagedBy $_.Manager -Path $location
                }
                else
                {
                    New-ADGroup -Name $_.UserGroup -GroupScope DomainLocal -ManagedBy $_.UserName -Path $location
                }
            }
            $group = Get-ADGroup $_.UserGroup
            Add-ADGroupMember $group.DistinguishedName $sam
            

            # Rename the object to a good looking name (otherwise you see
            # the 'ugly' shortened sAMAccountNames as a name in AD. This
            # can't be set right away (as sAMAccountName) due to the 20
            # character restriction
            if ($_.FirstName -and $_.LastName)
            {
                $newdn = (Get-ADUser $sam).DistinguishedName
                Rename-ADObject -Identity $newdn -NewName ($_.FirstName + " " + $_.LastName)
                Write-Host "[INFO]`t Renamed $($sam) to $($_.FirstName) $($_.LastName)`r`n"
                "[INFO]`t Renamed $($sam) to $($_.FirstName) $($_.LastName)`r`n" | Out-File $log -append
            }
          }
          Catch
          {
            Write-Host "[ERROR]`t Oops, something went wrong: $($_.Exception.Message)`r`n"
          }
        }
        Else
        {
          Write-Host "[SKIP]`t User $($sam) ($($_.FirstName) $($_.LastName)) already exists or returned an error!`r`n"
          "[SKIP]`t User $($sam) ($($_.FirstName) $($_.LastName)) already exists or returned an error!" | Out-File $log -append
        }
    }
    Else
    {
      Write-Host "[SKIP]`t User ($($_.FirstName) $($_.LastName)) will be skipped for processing!`r`n"
      "[SKIP]`t User ($($_.FirstName) $($_.LastName)) will be skipped for processing!" | Out-File $log -append
    }
    $i++
  }
  "--------------------------------------------" + "`r`n" | Out-File $log -append
}

Write-Host "STARTED SCRIPT`r`n"
Start-Commands
Write-Host "STOPPED SCRIPT"