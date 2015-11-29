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

        [AfterScenario]
        public void Cleanup()
        {
            Bus.Request<IUnregisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl, new UnregisterAccessPoint(_registeredAccessPoint.AccessPointId));
        }

        [Given(@"The following access point is registered")]
        [When(@"I register a new access point")]
        public void RegisterAccessPoint(AccessPoint accessPoint)
        {
            _registeredAccessPoint = accessPoint;
            Bus.Request<IRegisterAccessPoint, IVoidResult>(WellKnownQueues.AccessControl, new RegisterAccessPoint(_registeredAccessPoint));
        }

        [Given(@"I have the following access point")]
        public void IHaveTheFollowingAccessPoint(AccessPoint accessPoint)
        {
            _registeredAccessPoint = accessPoint;
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
    }
}