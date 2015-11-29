Feature: ManageAccessPoints
	Test access points management

Background: 
	Given I'm a Manager

Scenario: Allow user access
	When I register a new access point 
	| Site                    | Department     | AccessPointId                        | Name      | Description                               |
	| OU=USA,DC=Evriqum,DC=RU | Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |
	And I get a list of registered access points
	Then The result should contain an access point with name "TestPoint"`
