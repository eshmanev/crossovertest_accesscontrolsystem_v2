using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Configuration;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    public class UserGroupConsumer : IConsumer<IListUserGroups>
    {
        private readonly ILdapConfig _config;

        public UserGroupConsumer(ILdapConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        public Task Consume(ConsumeContext<IListUserGroups> context)
        {
            var path = _config.CombinePath(context.Message.Site);
            var entry = new DirectoryEntry(path, _config.UserName, _config.Password);

            var searcher = new DirectorySearcher(entry)
            {
                Filter = "(&(objectClass=group))",
                SearchScope = SearchScope.Subtree
            };

            var groups = searcher.FindAll().Cast<SearchResult>()
                                 .Select(x => new UserGroup(x.GetProperty("name")))
                                 .Cast<IUserGroup>()
                                 .ToArray();

            return context.RespondAsync(new ListUserGroupsResult(groups));
        }
    }
}