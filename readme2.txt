--------------------------------------------------------------------------------------------------------------
Build
--------------------------------------------------------------------------------------------------------------
1. Run build\ClickToBuild.cmd
2. Goto Artifacts folder and ensure that the build contains the following files:
	AccessControl.Service.AccessPoint.zip
	AccessControl.Service.LDAP.zip
     	AccessControl.Service.Notifications.zip
	AccessControl.Web.zip
	AccessControl.Client.zip
	AccessSimulator.zip
	.erlang.cookie
	common.ps1
	install_client_services.ps1
	install_server_services.ps1
	uninstall_client_services.ps1
	uninstall_server_services.ps1


--------------------------------------------------------------------------------------------------------------
INSTALLATION
--------------------------------------------------------------------------------------------------------------
The following steps should be executed for each node in your cluster

1. Install and configure RabbitMQ
1.1. Goto System -> Environment Variables and add RABBITMQ_BASE system variable to point your cluster drive (for me it's F:\RabbitMQ)
     This variable will be used by RabbitMQ installer for creating rabbitMQ databases and files.
1.2. Download and install Erlang http://www.erlang.org/download.html
1.3. Download and install RabbitMQ https://www.rabbitmq.com/download.html
1.4. Run RabbitMQ Command prompt and enable RabbitMQ management tool
	rabbitmq-plugins.bat enable rabbitmq_management
	rabbitmq-service.bat stop
	rabbitmq-service.bat install
     	rabbitmq-service.bat start
1.5. Copy and replace .erlang.cookie file from Artifacts folder to %WinDir%\ServiceProfiles\NetworkService and %WinDir%\.
1.6. Open the following ports:
	5672	This is the main AMQP port that clients use to talk to the broker.
	15672	The management web interface.
	4369	Used by EPMD (Erlang  Port Mapper Daemon). This makes sure that the nodes can find each other.
1.7. Allow connection for two binaries %Program Files%\erl7.1\bin\erl.exe and %Program Files%\erl7.1\erts-7.1\bin\erl.exe
     This allows Erlang to communicate between nodes. Optionally you can configure allowed ports instead of erl.exe. See RabbitMQ documentation.
1.8. Run RabbitMQ Command prompt, join the cluster and set mirroring policy
	rabbitmqctl stop_app
	rabbitmqctl join_cluster rabbit@YOUR_MASTER_NODE
	rabbitmqctl start_app
	rabbitmqctl set_policy ha-all "^ha\." "{""ha-mode"":""all""}"
     
Important note: YOUR_MASTER_NODE should be in upper case.

1.9. Delete guest access and add a new user which will be used by services. Because RabbitMQ is already mirrored these commands 
     can be executed one time on any of nodes. 
	rabbitmqctl add_user <username> <password>
     	rabbitmqctl set_permissions <username> ".*" ".*" ".*"
     	rabbitmqctl set_user_tags <username> management
     	rabbitmqctl delete_user guest

2. Create a new databases
2.1. Create a new empty MSSQL SERVER database. You should not create database schema, it will be created automatically later.

3. Install server services
3.1. Goto to folder with artifacts and copy all files on node
3.2. Run install_server_services.ps1 and enter required information: 
	- LDAP server and credentials
	- RabbitMQ server and credentials specified at 1.9 step
        - MSSQL Server connection string

     When the script finishes you can ensure that applications are installed and started.
     Goto Services snap-in and note that AccessControl.Service.LDAP, AccessControl.Service.AccessPoint, AccessControl.Service.Notifications services are started.
     Open IIS and note that AccessControl.Web website is installed.
