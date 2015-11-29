using System;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Commands;
using Shouldly;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class AccessPointSteps
    {
        private IAccessPoint _registeredAccessPoint;
        private IAccessPoint[] _listedAccessPoints;
        
        [Given(@"I have the following access point")]
        [When(@"I register a new access point")]
        public void RegisterAccessPoint(AccessPoint accessPoint)
        {
            _registeredAccessPoint = accessPoint;
            Bus.Request<IRegisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl, new RegisterAccessPoint(_registeredAccessPoint));

            // delete the access point after all tests
            CommonSteps.RegisterCleanup(() => Bus.Request<IUnregisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl, new UnregisterAccessPoint(_registeredAccessPoint.AccessPointId)));
        }

        [When(@"I unregister the access point with ID = ""(.*)""")]
        public void WhenIUnregisterTheAccessPointWithID(Guid accessPointId)
        {
            Bus.Request<IUnregisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl, new UnregisterAccessPoint(accessPointId));
        }


        [When(@"I get a list of registered access points")]
        public void WhenIGetAListOfRegisteredAccessPoints()
        {
            var result = Bus.Request<IListAccessPoints, IListAccessPointsResult>(WellKnownQueues.AccessControl, ListCommand.WithoutParameters);
            _listedAccessPoints = result.AccessPoints;
        }

        [Then(@"The result should contain an access point with name ""(.*)""`")]
        public void ThenTheResultShouldContainAnAccessPointWithName(string name)
        {
            _listedAccessPoints.ShouldNotBe(null);
            _listedAccessPoints.ShouldContain(x => x.Name == name);
        }

        [Then(@"The result should not contain an access point with name ""(.*)""`")]
        public void ThenTheResultShouldNotContainAnAccessPointWithName(string name)
        {
            _listedAccessPoints.ShouldNotBe(null);
            _listedAccessPoints.ShouldNotContain(x => x.Name == name);
        }

    }
}