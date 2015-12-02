Used design patterns:

MVC - web application
ORM - data access layer
Servant - see: RoleBasedDataFetcher provides common functionality for fetching data depending of roles of the current principal
Visitor - AccessRightsBase/IAccessRightsVisitor, AccessRuleBase/IAccessRuleVisitor
Abstract Factory - ISessionFactoryHolder, ISessionFactory
Lazy Initialization - SessionFactoryHolder - lazy initialization of session factory
Inversion of Control - Unity
Message bus - messaging
Microservices pattern - architecture

--------------------------------------------------------------------------------------------------------------
INSTALLATION
--------------------------------------------------------------------------------------------------------------
1. Select a server which will hosts the messaging queue.
1.1. Download and install Erlang on the server http://www.erlang.org/download.html
1.2. Download and install RabbitMQ on the server https://www.rabbitmq.com/download.html
1.3. Run RabbitMQ Command Prompt (this tool is added to the start menu during installation)
1.4. Within command prompt  execute the following commands: 
     
     rabbitmqctl add_user evgeny Test123
     rabbitmqctl set_permissions evgeny ".*" ".*" ".*"
     rabbitmqctl set_user_tags evgeny management
     rabbitmqctl delete_user guest

     Those commands will delete guest access and add a user, which is configured in web.config files.
     NOTE: If you use a different username/password, you will have to change web.config files accordingly.
1.5. Open port 5672 on RabbitMQ server. RabbitMQ uses this port for messaging by default.
2. Run build\ClickToBuild.cmd
3. Copy the files listed below from the artifacts folder to your server which is responsible for access control management and unzip them:
     AccessControl.Service.AccessPoint.zip
     AccessControl.Service.LDAP.zip
     AccessControl.Service.Notifications.zip

4. Copy AccessControl.Client.zip from the artifacts folder to your server which connected to vendor software and unzip it



Optional steps.
 - To enable RabbitMQ management tool you should execute the following commands.
     rabbitmq-plugins.bat enable rabbitmq_management
     rabbitmq-service.bat stop
     rabbitmq-service.bat install
     rabbitmq-service.bat start
     
   When it is done RabbitMQ management tool will be accessible on http://your-server-ip-address:15672 (ensure that the port is not blocked by firewall)


--------------------------------------------------------------------------------------------------------------
CONFIGURATION
--------------------------------------------------------------------------------------------------------------
1. The system requires access to the LDAP directory. 
   LDAP service should be authorized to fetch data from the LDAP directory.
   Client services are authenticated within the system using LDAP credentials. 
   
1.1. Create the following users in the LDAP directory:
     username    | password  | description   
     -------------------------------------------------------------------------------------------------------------------------------------------------------
     ldapservice | Test123   | LDAP service uses this user to fetch data from the LDAP directory (See: AccessControl.Services.LDAP\app.config)
     client1     | Test123   | Client access service uses this user to be authenticated within the system by default (See: AccessControl.Client\app.config)
     client2     | Test123   | Client access service uses this user to be authenticated within the system (optional)
     client3     | Test123   | Client access service uses this user to be authenticated within the system (optional)
     client4     | Test123   | Client access service uses this user to be authenticated within the system (optional)
     client5     | Test123   | Client access service uses this user to be authenticated within the system (optional)


1.2. Create the following user group in the LDAP directory:

     user group name        | user group members
     --------------------------------------------------------------------
     Access Control Clients | client1, client2, client3, client4, client5


NOTES:
 - You can create a temporary organizational structure in your Active Directory using a PowerShell script. 
   To achieve it, copy .\scripts\ldap_users.ps1 and .\scripts\import_create_ad_users.csv files to your server.
   Then run .\scripts\ldap_users.ps1 using Active Directory Module for Windows PowerShell.
   This script will create 4 organizational units: Head Office, European Actors, Chinese Actors and Client Services and add some users and user groups to them. 

 - Optionally you can update configuration files mentioned above with different credentials from the LDAP directory, but for testing purposes
   PowerShell script is a best choise.

 - Client services require Access Control Clients group to be authorized for fetching user names and user groups from the LDAP directory.
   It would be nice to create a manageable mappings between LDAP users and the system roles, but this is out of the scope of the current task.

2. Open the file AccessControl.Services.LDAP\app.config, navigate to the section <ldap ldapPath="...." /> and pick valid address of the LDAP directory server.
3. Create a new SQL Server database.
4. Open the file AccessControl.Services.AccessPoint\app.config, navigate to the section <connectionStrings> and pick valid connection string.
   By default, there is the following connection string: "Data Source=.\SQLEXPRESS;Initial Catalog=AccessControlSystem;Integrated Security=True"

   NOTE: The system automatically creates database schema at first run.



--------------------------------------------------------------------------------------------------------------
TROUBLESHOOTING
--------------------------------------------------------------------------------------------------------------
1. You may face a problem with WCF services, because Windows can block an URL if it has not been added to the access control list.
To fix the issue execute the following commands:

   netsh http add urlacl url=http://+:9981/AccessCheckService user=<your user name>
   netsh http add urlacl url=http://+:9981/AccessPointRegistry user=<your user name>


2. If you have a problem with IIS and ASP.NET 4.5. Execute the following command
   dism /online /enable-feature /all /featurename:IIS-ASPNET45