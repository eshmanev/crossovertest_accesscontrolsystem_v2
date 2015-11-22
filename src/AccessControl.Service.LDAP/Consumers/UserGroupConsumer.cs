using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Services;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
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

        public Task Consume(ConsumeContext<IListUserGroups> context)
        {
            var groups = _ldapService.FindUserGroupsByManager(context.UserName());
            return context.RespondAsync(ListCommand.Result(groups.ToArray()));
        }

        public Task Consume(ConsumeContext<IListUsersInGroup> context)
        {
            throw new System.NotImplementedException();
        }
    }
}