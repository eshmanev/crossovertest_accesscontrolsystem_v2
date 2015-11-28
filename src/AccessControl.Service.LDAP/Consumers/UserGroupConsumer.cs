using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service.LDAP.Services;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    /// <summary>
    ///     Represents a consumer of user groups.
    /// </summary>
    internal class UserGroupConsumer : IConsumer<IListUserGroups>, IConsumer<IListUsersInGroup>
    {
        private readonly ILdapService _ldapService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserGroupConsumer" /> class.
        /// </summary>
        /// <param name="ldapService">The LDAP service.</param>
        public UserGroupConsumer(ILdapService ldapService)
        {
            Contract.Requires(ldapService != null);
            _ldapService = ldapService;
        }

        /// <summary>
        ///     Returns a list of user groups.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListUserGroups> context)
        {
            var fetcher = RoleBasedDataFetcher.Create(_ldapService.ListUserGroups, manager => _ldapService.FindUserGroupsByManager(manager));
            var groups = fetcher.Execute();
            return context.RespondAsync(ListCommand.UserGroupsResult(groups.ToArray()));
        }

        /// <summary>
        ///     Returns a list of users in the specified group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListUsersInGroup> context)
        {
            var fetcher = RoleBasedDataFetcher.Create(
                () => _ldapService.GetUsersInGroup(context.Message.UserGroupName),
                manager =>
                {
                    var allUsers = _ldapService.GetUsersInGroup(context.Message.UserGroupName);
                    var allowed = _ldapService.FindUsersByManager(manager);
                    return allUsers.Where(x => allowed.Any(y => y.UserName == x.UserName));
                });

            var users = fetcher.Execute();
            return context.RespondAsync(ListCommand.UsersInGroupResult(users.ToArray()));
        }
    }
}