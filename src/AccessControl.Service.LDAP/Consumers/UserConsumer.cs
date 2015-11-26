using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Services;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    /// <summary>
    ///     Represents a consumer of LDAP users.
    /// </summary>
    internal class UserConsumer : IConsumer<ICheckCredentials>, IConsumer<IFindUserByName>, IConsumer<IListUsers>
    {
        private readonly ILdapService _ldapService;
        

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserConsumer" /> class.
        /// </summary>
        /// <param name="ldapService">The LDAP service.</param>
        public UserConsumer(ILdapService ldapService)
        {
            Contract.Requires(ldapService != null);
            _ldapService = ldapService;
        }

        /// <summary>
        ///     Checks the specified credentials.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<ICheckCredentials> context)
        {
            var result = _ldapService.CheckCredentials(context.Message.UserName, context.Message.Password);
            if (!result)
                return context.RespondAsync(new CheckCredentialsResult(null));

            var user = _ldapService.FindUserByName(context.Message.UserName);
            return context.RespondAsync(new CheckCredentialsResult(user));
        }

        /// <summary>
        ///     Searches for a user by name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IFindUserByName> context)
        {
            var user = _ldapService.FindUserByName(context.Message.UserName);
            return context.RespondAsync<IFindUserByNameResult>(new FindUserByNameResult(user));
        }

        /// <summary>
        ///     Returns a list of users managed by the logged in user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListUsers> context)
        {
            IEnumerable<IUser> users;

            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
                users = _ldapService.ListUsers();
            else if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
                users = _ldapService.FindUsersByManager(context.UserName());
            else
                users = Enumerable.Empty<IUser>();

            return context.RespondAsync(ListCommand.UsersResult(users.ToArray()));
        }
    }
}