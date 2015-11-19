﻿using System.Diagnostics.Contracts;
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
    public class FindUserConsumer : IConsumer<IFindUserByName>, IConsumer<IListUsers>
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

            var user = result == null ? null : ConvertUser(result);
            return context.RespondAsync<IFindUserByNameResult>(new FindUserByNameResult(user));
        }

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