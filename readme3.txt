--------------------------------------------------------------------------------------------------------------
Reports
--------------------------------------------------------------------------------------------------------------
This section describes how to configure daily reports.
Before you start ensure that the database schema is generated (you need to run AccessControl.Web application and login) 
To proceed the steps below you need MSSQL Management Studio and MSSQL Reporting Services installed.

1. Prepare database.
1.1. Run MSSQL Management Studio and open reporting\report_functions.sql script.
1.2. Replace values of @AD_UserName and @AD_Password variables with your Active Directory credentials.
1.3. Execute the script
2. Configure Report Manager
2.1. Open Report Manager web application in browser and load Report Builder.
2.2. In Report Builder open reporting\department-wise-activity.rdl file and upload that file on Report Manager.
2.3. Navigate to Report Manager, find the uploaded file and open context menu and click Manage item.
2.4. Click on Subscriptions and then create a new data-driven subscriptions
2.5. 