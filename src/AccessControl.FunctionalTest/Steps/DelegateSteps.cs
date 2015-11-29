using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class DelegateSteps
    {
        [When(@"I grant management rights to my employee")]
        public void WhenIGrantManagementRightsToMyEmployee()
        {
            var userName = AppSettings.ManagedUserName;
            Bus.Request<IGrantManagementRights, IVoidResult>(WellKnownQueues.AccessControl, new GrantRevokeManagementRights(userName));
        }

        [When(@"I revoke management rights from my employee")]
        public void WhenIRevokeManagementRightsFromMyEmployee()
        {
            var userName = AppSettings.ManagedUserName;
            Bus.Request<IRevokeManagementRights, IVoidResult>(WellKnownQueues.AccessControl, new GrantRevokeManagementRights(userName));
        }
    }
}