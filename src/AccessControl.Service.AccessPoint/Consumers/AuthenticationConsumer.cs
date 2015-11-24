using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using AccessControl.Data.Entities;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class AuthenticationConsumer : IConsumer<IAuthenticateUser>
    {
        private readonly IRequestClient<ICheckCredentials, ICheckCredentialsResult> _checkCredentialsRequest;
        private readonly IEncryptor _encryptor;
        private readonly IRepository<DelegatedRights> _delegatedRightsRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticationConsumer" /> class.
        /// </summary>
        /// <param name="checkCredentialsRequest">The check credentials request.</param>
        /// <param name="encryptor">The encryptor.</param>
        /// <param name="delegatedRightsRepository">The delegated rights repository.</param>
        public AuthenticationConsumer(IRequestClient<ICheckCredentials, ICheckCredentialsResult> checkCredentialsRequest,
                                      IEncryptor encryptor,
                                      IRepository<DelegatedRights> delegatedRightsRepository)
        {
            Contract.Requires(checkCredentialsRequest != null);
            Contract.Requires(encryptor != null);
            Contract.Requires(delegatedRightsRepository != null);

            _checkCredentialsRequest = checkCredentialsRequest;
            _encryptor = encryptor;
            _delegatedRightsRepository = delegatedRightsRepository;
        }

        /// <summary>
        ///     Authenticates the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAuthenticateUser> context)
        {
            var checkResult = await _checkCredentialsRequest.Request(new CheckCredentials(context.Message.UserName, context.Message.Password));
            if (!checkResult.Valid)
            {
                await context.RespondAsync(AuthenticateUserResult.Failed());
                return;
            }

            var user = checkResult.User;
            var roles = new List<string>();
            if (user.IsManager)
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

            // select delegated privileges
            var delegatedRights = _delegatedRightsRepository.Filter(x => x.Grantee == user.UserName);
            var onBehalfOf = delegatedRights.Select(x => x.Grantor).ToArray();

            // create a ticket
            var ticket = new Ticket(user, roles.ToArray(), onBehalfOf);
            var encryptedTicket = _encryptor.Encrypt(ticket);
            await context.RespondAsync(new AuthenticateUserResult(true, encryptedTicket));
        }
    }
}