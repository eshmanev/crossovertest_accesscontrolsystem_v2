using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using AccessControl.LDAP.Service.Configuration;
using MassTransit;

namespace AccessControl.LDAP.Service.Consumers
{
    public class FindUserConsumer : IConsumer<IFindUserByName>
    {
        private readonly ILdapConfig _config;

        public FindUserConsumer(ILdapConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        public Task Consume(ConsumeContext<IFindUserByName> context)
        {
            var entry = new DirectoryEntry(_config.LdapPath, _config.UserName, _config.Password);
            var searcher = new DirectorySearcher(entry) {Filter = $"(sAMAccountName={context.Message.UserName})"};
            var result = searcher.FindOne();
            return result == null
                       ? context.RespondAsync<FindUserByNameResult>(null)
                       : context.RespondAsync(
                           new FindUserByNameResult(context.Message.UserName)
                           {
                               DisplayName = result.Properties["displayname"].Count > 0 ? (string) result.Properties["displayname"][0] : "Ivan",
                               PhoneNumber = result.Properties["telephonenumber"].Count > 0 ? (string) result.Properties["telephonenumber"][0] : string.Empty,
                               Email = result.Properties["mail"].Count > 0 ? (string) result.Properties["mail"][0] : string.Empty
                           });
        }
    }
}