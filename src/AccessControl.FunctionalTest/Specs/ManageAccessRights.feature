Feature: ManageAccessRights
	Test access rights management

Background: 
	Given I have started the AccessPoint service
	And I have started the LDAP service
	And I have started the Client service

Scenario: Allow user access
	Given I have the following access point
	| Site                    | Department     | AccessPointId                        | Name      | Description                               |
	| OU=USA,DC=Evriqum,DC=RU | Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |
	And I have the following user
	| UserName | BiometricHash |
	| testuser | hash123       |