Feature: ManageAccessRights
	Test access rights management

Background: 
	Given I'm a Manager
	And I have the following access point
	| Department     | AccessPointId                        | Name      | Description                               |
	| Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |
	And My employee has the following biometric hash "testhash12345"

Scenario: Allow user access
	When I grant access rights to access point with ID = "00FB8A36-B9A0-42AC-9F95-C785F74A14B7" for my employee
	And My employee tries to access the access point with ID = "00FB8A36-B9A0-42AC-9F95-C785F74A14B7"
	Then The access should be allowed
	And A successful log entry should be created with my employee and access point "TestPoint"
	
Scenario: Deny user access
	When I deny access rights to access point with ID = "00FB8A36-B9A0-42AC-9F95-C785F74A14B7" for my employee
	And My employee tries to access the access point with ID = "00FB8A36-B9A0-42AC-9F95-C785F74A14B7"
	Then The access should be denied
	And A failed log entry should be created with my employee and access point "TestPoint"
	And I should be notified by email
	And I should be notified by sms