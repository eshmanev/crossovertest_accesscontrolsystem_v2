using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Data;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class AuthenticationConsumer : IConsumer<IAuthenticateUser>
    {
        private readonly IRequestClient<ICheckCredentials, ICheckCredentialsResult> _checkCredentialsRequest;
        private readonly IEncryptor _encryptor;
        private readonly IDatabaseContext _databaseContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticationConsumer" /> class.
        /// </summary>
        /// <param name="checkCredentialsRequest">The check credentials request.</param>
        /// <param name="encryptor">The encryptor.</param>
        /// <param name="databaseContext">The database context.</param>
        public AuthenticationConsumer(IRequestClient<ICheckCredentials, ICheckCredentialsResult> checkCredentialsRequest,
                                      IEncryptor encryptor,
                                      IDatabaseContext databaseContext)
        {
            Contract.Requires(checkCredentialsRequest != null);
            Contract.Requires(encryptor != null);
            Contract.Requires(databaseContext != null);

            _checkCredentialsRequest = checkCredentialsRequest;
            _encryptor = encryptor;
            _databaseContext = databaseContext;
        }

        /// <summary>
        ///     Authenticates the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAuthenticateUser> context)
        {
            var parts = context.Message.UserName.Split('\\');
            if (parts.Length != 2)
            {
                await context.RespondAsync(AuthenticateUserResult.Failed());
                return;
            }

            var checkResult = await _checkCredentialsRequest.Request(new CheckCredentials(parts[0], parts[1], context.Message.Password));
            if (!checkResult.Valid)
            {
                await context.RespondAsync(AuthenticateUserResult.Failed());
                return;
            }

            var user = checkResult.User;
            var roles = new List<string>();

            // select delegated privileges
            var delegatedRights = _databaseContext.DelegatedRights.Filter(x => x.Grantee == user.UserName);
            var onBehalfOf = delegatedRights.Select(x => x.Grantor).ToArray();


            // add manager role
            if (user.IsManager || onBehalfOf.Length > 0)
            {
                roles.Add(WellKnownRoles.Manager);
            }

            /* IMPORTANT NOTE:
               This is a temporary solution. User group name cannot be hardcoded. 
               We should provide a feature that allows to map User Groups to the application roles.
               But this is postponded, because of time limit.
            */
            var userGroups = user.Groups;
            if (userGroups.Any(x => x == "Access Control Clients"))
            {
                roles.Add(WellKnownRoles.ClientService);
            }

            // create a ticket
            var ticket = new Ticket(parts[0], user, roles.ToArray(), onBehalfOf);
            var encryptedTicket = _encryptor.Encrypt(ticket);
            await context.RespondAsync(new AuthenticateUserResult(true, encryptedTicket, user));
        }
    }
}