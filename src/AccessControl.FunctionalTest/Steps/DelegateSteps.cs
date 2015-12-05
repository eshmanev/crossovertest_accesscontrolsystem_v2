using System.Linq;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using TechTalk.SpecFlow;

namespace AccessControl.FunctionalTest.Steps
{
    [Binding]
    public class DelegateSteps
    {
        [When(@"I grant management rights to my employee")]
        public void WhenIGrantManagementRightsToMyEmployee()
        {
            var cname = AppSettings.ManagedUserName;
            var usersResult = Bus.Request<IListUsers, IListUsersResult>(WellKnownQueues.Ldap, ListCommand.WithoutParameters);
            var user = usersResult.Users.Single(x => x.UserName.Contains(cname));
            Bus.Request<IGrantManagementRights, IVoidResult>(WellKnownQueues.AccessControl, new GrantRevokeManagementRights(user.UserName));
        }

        [When(@"I revoke management rights from my employee")]
        public void WhenIRevokeManagementRightsFromMyEmployee()
        {
            var cname = AppSettings.ManagedUserName;
            var usersResult = Bus.Request<IListUsers, IListUsersResult>(WellKnownQueues.Ldap, ListCommand.WithoutParameters);
            var user = usersResult.Users.Single(x => x.UserName.Contains(cname));
            Bus.Request<IRevokeManagementRights, IVoidResult>(WellKnownQueues.AccessControl, new GrantRevokeManagementRights(user.UserName));
        }
    }
}