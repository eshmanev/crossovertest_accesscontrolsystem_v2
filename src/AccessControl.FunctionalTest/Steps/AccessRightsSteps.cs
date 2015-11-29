using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.FunctionalTest.AccessCheckServiceProxy;
using Shouldly;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class AccessRightsSteps
    {
        private bool accessAllowed;

        [When(@"I grant access rights to access point with ID = ""(.*)"" for my employee")]
        public void WhenIGrantAccessRightsToAccessPointWithIDForMyEmployee(Guid accessPointId)
        {
            var employee = AppSettings.ManagedUserName;
            Bus.Request<IAllowUserAccess, IVoidResult>(WellKnownQueues.AccessControl, new AllowDenyUserAccess(accessPointId, employee));

            // this is just to ensure that all necessary events are delivered and processed
            Thread.Sleep(1000);
        }

        [When(@"I deny access rights to access point with ID = ""(.*)"" for my employee")]
        public void WhenIDenyAccessRightsToAccessPointWithIDForMyEmployee(Guid accessPointId)
        {
            var employee = AppSettings.ManagedUserName;
            Bus.Request<IDenyUserAccess, IVoidResult>(WellKnownQueues.AccessControl, new AllowDenyUserAccess(accessPointId, employee));

            // this is just to ensure that all necessary events are delivered and processed
            Thread.Sleep(1000);
        }

        [When(@"My employee tries to access the access point with ID = ""(.*)""")]
        public void WhenMyEmployeeTriesToAccessTheAccessPointWithID(Guid accessPointId)
        {
            // get user's hash
            var result = Bus.Request<IListUsersBiometric, IListUsersBiometricResult>(WellKnownQueues.AccessControl, ListCommand.WithoutParameters);
            var userHash = result.Users.Single(x => x.UserName == AppSettings.ManagedUserName).BiometricHash;

            // try access the access point
            var client = new AccessCheckServiceProxy.AccessCheckServiceClient();
            accessAllowed = client.TryPass(new CheckAccess { AccessPointId = accessPointId, UserHash = userHash });

            // this is just to ensure that all necessary events are delivered and processed
            Thread.Sleep(1000);
        }


        [Then(@"The access should be allowed")]
        public void ThenTheAccessShouldBeAllowed()
        {
            accessAllowed.ShouldBe(true);
        }

        [Then(@"The access should be denied")]
        public void ThenTheAccessShouldBeDenied()
        {
            accessAllowed.ShouldBe(false);
        }

    }
}