using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using AccessControl.Contracts.Dto;
using AccessControl.Service.LDAP.Configuration;

namespace AccessControl.Service.LDAP.Services
{
    internal class LdapService : ILdapService
    {
        private readonly ConcurrentDictionary<string, ILdapService> _directoryServices = new ConcurrentDictionary<string, ILdapService>();
        private readonly ILdapConfig _config;

        public LdapService(ILdapConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        private ILdapService Current
        {
            get
            {
                var domain = GetDomain(Thread.CurrentPrincipal.UserName());
                return Get(domain);
            }
        }

        private string GetDomain(string userName)
        {
            var parts = userName.Split('\\');
            if (parts.Length != 2)
            {
                return null;
            }
            return parts[0];
        }

        private ILdapService Get(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return EmptyDirectoryService.Instance;

            var directoryConfig = _config.Directories.FirstOrDefault(
                x => string.Equals(x.DomainName, domain, StringComparison.InvariantCultureIgnoreCase) ||
                     string.Equals(x.Alias, domain, StringComparison.InvariantCultureIgnoreCase));

            if (directoryConfig == null)
                return EmptyDirectoryService.Instance;

            return _directoryServices.GetOrAdd(directoryConfig.DomainName, x => new DirectoryService(directoryConfig));
        }

        public bool CheckCredentials(string userName, string password, out IUser user)
        {
            var domain = GetDomain(userName);
            if (domain == null)
            {
                user = null;
                return false;
            }

            return Get(domain).CheckCredentials(userName, password, out user);
        }

        public IEnumerable<IDepartment> ListDepartments()
        {
            return Current.ListDepartments();
        }

        public IEnumerable<IDepartment> FindDepartmentsByManager(string managerName)
        {
            return Current.FindDepartmentsByManager(managerName);
        }

        public IUser FindUserByName(string userName)
        {
            return Current.FindUserByName(userName);
        }

        public IEnumerable<IUserGroup> FindUserGroupsByManager(string managerName)
        {
            return Current.FindUserGroupsByManager(managerName);
        }

        public IEnumerable<IUser> FindUsersByManager(string managerName)
        {
            return Current.FindUsersByManager(managerName);
        }

        public IEnumerable<IUserGroup> GetUserGroups(string userName)
        {
            return Current.GetUserGroups(userName);
        }

        public IEnumerable<IUser> GetUsersInGroup(string userGroupName)
        {
            return Current.GetUsersInGroup(userGroupName);
        }

        public IEnumerable<IUserGroup> ListUserGroups()
        {
            return Current.ListUserGroups();
        }

        public IEnumerable<IUser> ListUsers()
        {
            return Current.ListUsers();
        }
    }
}