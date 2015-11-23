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
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    /// <summary>
    ///     Represents a consumer of LDAP users.
    /// </summary>
    internal class UserConsumer : IConsumer<IAuthenticateUser>, IConsumer<IFindUserByName>, IConsumer<IListUsers>
    {
        private readonly ILdapService _ldapService;
        private readonly IEncryptor _encryptor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserConsumer" /> class.
        /// </summary>
        /// <param name="ldapService">The LDAP service.</param>
        /// <param name="encryptor">The encryptor.</param>
        public UserConsumer(ILdapService ldapService, IEncryptor encryptor)
        {
            Contract.Requires(ldapService != null);
            Contract.Requires(encryptor != null);
            _ldapService = ldapService;
            _encryptor = encryptor;
        }

        /// <summary>
        ///     Consumes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IAuthenticateUser> context)
        {
            var result = _ldapService.Authenticate(context.Message.UserName, context.Message.Password);
            if (!result)
            {
                return context.RespondAsync(AuthenticateUserResult.Failed());
            }

            var user = _ldapService.FindUserByName(context.Message.UserName);
            var roles = new List<string>();
            if (_ldapService.FindUsersByManager(context.Message.UserName).Any())
                roles.Add(WellKnownRoles.Manager);

            /* IMPORTANT NOTE:
               This is a temporary solution. User group name cannot be hardcoded. 
               We should provide a feature that allows to map User Groups to the application roles.
               But this is postponded, because of time limit.
            */
            var userGroups = _ldapService.GetUserGroups(user.UserName);
            if (userGroups.Any(x => x.Name == "Access Control Clients"))
                roles.Add(WellKnownRoles.ClientService);

            var ticket = new Ticket(user, roles.ToArray());
            var encryptedTicket = _encryptor.Encrypt(ticket);
            return context.RespondAsync(new AuthenticateUserResult(true, encryptedTicket));
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