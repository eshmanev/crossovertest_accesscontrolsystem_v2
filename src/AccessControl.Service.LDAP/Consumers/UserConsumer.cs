using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service.LDAP.Services;
using AccessControl.Service.Security;
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
            IUser user;
            return context.RespondAsync(!_ldapService.CheckCredentials(context.Message.Domain, context.Message.UserName, context.Message.Password, out user) 
                ? new CheckCredentialsResult(null) 
                : new CheckCredentialsResult(user));
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
            var fetcher = RoleBasedDataFetcher.Create(_ldapService.ListUsers, x => _ldapService.FindUsersByManager(x));
            var users = fetcher.Execute();
            return context.RespondAsync(ListCommand.UsersResult(users.ToArray()));
        }
    }
}