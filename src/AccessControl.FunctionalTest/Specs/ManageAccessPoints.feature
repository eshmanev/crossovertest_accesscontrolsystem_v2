Feature: ManageAccessPoints
	Test access points management

Background: 
	Given I'm a Manager

Scenario: Register a new access point
	When I register a new access point 
	| Department     | AccessPointId                        | Name      | Description                               |
	| Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |
	And I get a list of registered access points
	Then The result should contain an access point with name "TestPoint"`

Scenario: Unregister an access point
	Given I have the following access point
	| Department     | AccessPointId                        | Name      | Description                               |
	| Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |
	When I unregister the access point with ID = "00FB8A36-B9A0-42AC-9F95-C785F74A14B7"
	And I get a list of registered access points
	Then The result should not contain an access point with name "TestPoint"`