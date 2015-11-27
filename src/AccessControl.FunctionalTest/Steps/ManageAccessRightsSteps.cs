using System;
using System.Threading;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.FunctionalTest.Rows;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class ManageAccessRightsSteps
    {
        private AccessPoint _accessPoint;

        [AfterScenario]
        public void Cleanup()
        {
            BusControl.GetBus().Publish(new UnregisterAccessPoint(_accessPoint.AccessPointId));
        }

        [Given(@"I have the following access point")]
        public void GivenIHaveTheFollowingAccessPoint(AccessPoint accessPoint)
        {
            _accessPoint = accessPoint;
            BusControl.GetBus().Publish(new RegisterAccessPoint(_accessPoint));
            Thread.Sleep(30000);
        }
        
        [Given(@"I have the following user")]
        public void GivenIHaveTheFollowingUser(Table table)
        {
           // ScenarioContext.Current.Pending();
        }

        
    }
}
