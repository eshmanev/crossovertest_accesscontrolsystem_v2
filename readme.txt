
1. If there is a WCF issue because an URL is blocked by Windows, then just execute the following command
netsh http add urlacl url=http://+:9981/AccessCheckService user=<your user name>
netsh http add urlacl url=http://+:9981/AccessPointRegistry user=<your user name>

2. Create temprorary certificate for development
makecert -sv AccessPoint.pvk -n "cn=Access Point Development" AccessPoint.cer -b 01/01/2010 -e 12/31/2016 -r