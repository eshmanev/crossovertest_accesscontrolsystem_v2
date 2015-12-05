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
            Bus.Request<IGrantManagementRights, IVoidResult>(WellKnownQueues.AccessControl, new GrantRevokeManagementRights(AppSettings.ManagedUserName));
        }

        [When(@"I revoke management rights from my employee")]
        public void WhenIRevokeManagementRightsFromMyEmployee()
        {
            Bus.Request<IRevokeManagementRights, IVoidResult>(WellKnownQueues.AccessControl, new GrantRevokeManagementRights(AppSettings.ManagedUserName));
        }
    }
}