﻿Feature: ManageAccessRights
	Test access rights management

Background: 
	Given I'm a Manager

Scenario: Allow user access
	Given The following access point is registered
	| Site                    | Department     | AccessPointId                        | Name      | Description                               |
	| OU=USA,DC=Evriqum,DC=RU | Top Management | 00FB8A36-B9A0-42AC-9F95-C785F74A14B7 | TestPoint | This is an access point for test purposes |
	