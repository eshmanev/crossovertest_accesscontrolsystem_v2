using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
                var domain = Thread.CurrentPrincipal.Domain();
                return Get(domain);
            }
        }

        private ILdapService Get(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain))
                return EmptyDirectoryService.Instance;

            return _directoryServices.GetOrAdd(
                    domain,
                    x =>
                    {
                        var directoryConfig = _config.Directories[x];
                        return directoryConfig != null ? new DirectoryService(directoryConfig) as ILdapService : EmptyDirectoryService.Instance;
                    });
        }

        public bool CheckCredentials(string domain, string userName, string password, out IUser user)
        {
            return Get(domain).CheckCredentials(domain, userName, password, out user);
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

        public bool ValidateDepartment(string site, string department)
        {
            return Current.ValidateDepartment(site, department);
        }
    }
}