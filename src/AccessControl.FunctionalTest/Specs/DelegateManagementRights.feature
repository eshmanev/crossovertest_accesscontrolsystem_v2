Feature: DelegateManagementRights
	Test delegated management rights

Background: 
	Given I'm a Manager
	Given I have the following access point
	| Site                    | Department     | AccessPointId                        | Name      | Description                               |
	| OU=USA,DC=Evriqum,DC=RU | Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |

Scenario: Grant management rights
	When I grant management rights to my employee
	And My employee get authenticated
	And My employee tries to list access points
	Then The result should contain an access point with name "TestPoint"`
	
Scenario: Revoke management rights
	When I revoke management rights from my employee
	And My employee get authenticated
	And My employee tries to list access points
	Then The result should not contain an access point with name "TestPoint"`
	