using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Configuration;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    /// <summary>
    ///     Represents a consumer of LDAP users.
    /// </summary>
    public class UserConsumer : IConsumer<IFindUserByName>, IConsumer<IListUsers>
    {
        private readonly ILdapConfig _config;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserConsumer" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public UserConsumer(ILdapConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        /// <summary>
        ///     Searches for a user by name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IFindUserByName> context)
        {
            var entry = new DirectoryEntry(_config.LdapPath, _config.UserName, _config.Password);
            var searcher = new DirectorySearcher(entry) {Filter = $"(sAMAccountName={context.Message.UserName})"};
            var result = searcher.FindOne();

            var user = result == null ? null : ConvertUser(result);
            return context.RespondAsync<IFindUserByNameResult>(new FindUserByNameResult(user));
        }

        /// <summary>
        ///     Returns a list of users.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListUsers> context)
        {
            var entry = new DirectoryEntry(_config.CombinePath(context.Site()), _config.UserName, _config.Password);
            var searcher = new DirectorySearcher(entry) {Filter = $"(&(objectClass=user)(department={context.Department()}))"};
            var users = searcher.FindAll().Cast<SearchResult>().Select(ConvertUser).ToArray();
            return context.RespondAsync(ListCommand.Result(users));
        }

        private IUser ConvertUser(SearchResult result)
        {
            var userName = result.GetProperty("samaccountname");
            return new User(result.GetDirectoryEntry().Parent.GetProperty("distinguishedName"), userName)
            {
                DisplayName = result.GetProperty("displayname") ?? userName,
                PhoneNumber = result.GetProperty("telephonenumber"),
                Email = result.GetProperty("mail"),
                Department = result.GetProperty("department")
            };
        }
    }
}