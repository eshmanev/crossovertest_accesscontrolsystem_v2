using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service.LDAP.Services;
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
            IEnumerable<IUserGroup> groups;

            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                groups = _ldapService.ListUserGroups();
            }
            else if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                groups = _ldapService.FindUserGroupsByManager(context.UserName());
            }
            else
            {
                groups = Enumerable.Empty<IUserGroup>();
            }

            return context.RespondAsync(ListCommand.UserGroupsResult(groups.ToArray()));
        }

        /// <summary>
        ///     Returns a list of users in the specified group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListUsersInGroup> context)
        {
            IEnumerable<IUser> users;

            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                users = _ldapService.GetUsersInGroup(context.Message.UserGroupName);
            }
            else if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                users = _ldapService.GetUsersInGroup(context.Message.UserGroupName);
                var allowed = _ldapService.FindUsersByManager(context.UserName());
                users = users.Where(x => allowed.Any(y => y.UserName == x.UserName));
            }
            else
            {
                users = Enumerable.Empty<IUser>();
            }

            return context.RespondAsync(ListCommand.UsersInGroupResult(users.ToArray()));
        }
    }
}