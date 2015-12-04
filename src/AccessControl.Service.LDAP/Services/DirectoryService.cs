using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Linq;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Service.LDAP.Configuration;
using AccessControl.Service.LDAP.Helpers;

namespace AccessControl.Service.LDAP.Services
{
    internal class DirectoryService : ILdapService
    {
        private readonly IDirectoryConfig _config;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectoryService" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public DirectoryService(IDirectoryConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        /// <summary>
        ///     Finds the user by name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public IUser FindUserByName(string userName)
        {
            var result = FindUserByNameCore(userName);
            return result != null ? ConvertUser(result) : null;
        }

        /// <summary>
        ///     Gets the user groups in which the specified user is member of.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public IEnumerable<IUserGroup> GetUserGroups(string userName)
        {
            return GetUserGroupsCore(userName).Select(x => new UserGroup(x));
        }

        /// <summary>
        ///     Gets the users in the specified group.
        /// </summary>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <returns></returns>
        public IEnumerable<IUser> GetUsersInGroup(string userGroupName)
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = $"(&(objectClass=group)(name={userGroupName}))";
                    searcher.SearchScope = SearchScope.Subtree;

                    var groupResult = searcher.FindOne();
                    var userNames = groupResult.Properties["member"].OfType<string>();
                    return userNames
                        .Select(
                            x =>
                            {
                                searcher.Filter = $"(distinguishedname={x})";
                                return ConvertUser(searcher.FindOne());
                            })
                        .ToArray();
                }
            }
        }

        /// <summary>
        ///     Finds the users by manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        public IEnumerable<IUser> FindUsersByManager(string managerName)
        {
            var manager = FindUserByNameCore(managerName);
            if (manager == null)
            {
                return Enumerable.Empty<IUser>();
            }

            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(&(objectClass=user)(manager={manager.GetProperty("distinguishedName")}))"})
                {
                    return searcher.FindAll().Cast<SearchResult>().Select(ConvertUser);
                }
            }
        }

        /// <summary>
        ///     Lists the users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IUser> ListUsers()
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = "(objectClass=user)"})
                {
                    return searcher.FindAll().Cast<SearchResult>().Select(ConvertUser);
                }
            }
        }

        /// <summary>
        ///     Validates the department.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="department">The department.</param>
        /// <returns></returns>
        public bool ValidateDepartment(string site, string department)
        {
            var path = _config.CombinePath(site);
            using (var directoryEntiry = CreateEntry(path))
            {
                using (var searcher = new DirectorySearcher(directoryEntiry) {Filter = "(objectClass=user)"})
                {
                    searcher.PropertiesToLoad.Add("department");
                    return searcher.FindAll().Cast<SearchResult>().Any(x => x.GetProperty("department") == department);
                }
            }
        }

        /// <summary>
        ///     Finds the user groups by manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        public IEnumerable<IUserGroup> FindUserGroupsByManager(string managerName)
        {
            var manager = FindUserByNameCore(managerName);
            if (manager == null)
            {
                return Enumerable.Empty<IUserGroup>();
            }

            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = $"(&(objectClass=group)(managedby={manager.GetProperty("distinguishedName")}))";
                    searcher.SearchScope = SearchScope.Subtree;

                    return searcher.FindAll().Cast<SearchResult>()
                                   .Select(x => new UserGroup(x.GetProperty("name")))
                                   .Cast<IUserGroup>()
                                   .ToArray();
                }
            }
        }

        /// <summary>
        ///     Lists the user groups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IUserGroup> ListUserGroups()
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = "(objectClass=group)";
                    searcher.SearchScope = SearchScope.Subtree;

                    return searcher.FindAll().Cast<SearchResult>()
                                   .Select(x => new UserGroup(x.GetProperty("name")))
                                   .Cast<IUserGroup>()
                                   .ToArray();
                }
            }
        }

        /// <summary>
        ///     Checks the specified credentials.
        /// </summary>
        /// <param name="domain">Domain name.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns></returns>
        public bool CheckCredentials(string domain, string userName, string password, out IUser user)
        {
            try
            {
                using (var entry = new DirectoryEntry(_config.Url, userName, password))
                {
                    entry.AuthenticationType = AuthenticationTypes.Secure;
                    // ReSharper disable once UnusedVariable
                    var nativeObject = entry.NativeObject;
                    user = FindUserByName(userName);
                    return true;
                }
            }
            catch
            {
                user = null;
                return false;
            }
        }

        /// <summary>
        ///     Lists the departments.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDepartment> ListDepartments()
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = "(objectClass=user)"})
                {
                    searcher.PropertiesToLoad.Add("department");

                    var departments = searcher.FindAll()
                                              .Cast<SearchResult>()
                                              .Select(ConvertDepartment)
                                              .Where(x => x != null);

                    return departments.Distinct();
                }
            }
        }

        /// <summary>
        ///     Finds the departments by manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        public IEnumerable<IDepartment> FindDepartmentsByManager(string managerName)
        {
            var manager = FindUserByNameCore(managerName);
            if (manager == null)
            {
                return Enumerable.Empty<IDepartment>();
            }

            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(&(objectClass=user)(manager={manager.GetProperty("distinguishedName")}))"})
                {
                    searcher.PropertiesToLoad.Add("department");

                    var departments = searcher.FindAll()
                                              .Cast<SearchResult>()
                                              .Select(ConvertDepartment)
                                              .Where(x => x != null);

                    return departments.Distinct();
                }
            }
        }

        private IDepartment ConvertDepartment(SearchResult result)
        {
            var siteDistinguished = result.GetDirectoryEntry().Parent.GetProperty("distinguishedName");
            var site = result.GetDirectoryEntry().Parent.GetProperty("name");
            var department = result.GetProperty("department");
            if (string.IsNullOrWhiteSpace(siteDistinguished) || string.IsNullOrWhiteSpace(site) || string.IsNullOrWhiteSpace(department))
            {
                return null;
            }

            return new Department(siteDistinguished, site, department);
        }

        private IUser ConvertUser(SearchResult result)
        {
            var userName = result.GetProperty("samaccountname");
            var managerName = result.GetProperty("manager");
            var manager = managerName != null ? FindUserByDistinguishedName(managerName) : null;
            return new User(result.GetDirectoryEntry().Parent.GetProperty("distinguishedName"), userName, GetUserGroupsCore(userName).ToArray())
            {
                DisplayName = result.GetProperty("displayname") ?? userName,
                PhoneNumber = result.GetProperty("telephonenumber"),
                Email = result.GetProperty("mail"),
                Department = result.GetProperty("department"),
                IsManager = FindUsersByManager(userName).Any(),
                ManagerName = manager?.GetProperty("samaccountname")
            };
        }

        private SearchResult FindUserByDistinguishedName(string distinguishedName)
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(distinguishedName={distinguishedName})"})
                {
                    return searcher.FindOne();
                }
            }
        }

        private SearchResult FindUserByNameCore(string userName)
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(sAMAccountName={userName})"})
                {
                    return searcher.FindOne();
                }
            }
        }

        private IEnumerable<string> GetUserGroupsCore(string userName)
        {
            var result = FindUserByNameCore(userName);
            if (result == null)
            {
                yield break;
            }

            foreach (var property in result.Properties["memberOf"].OfType<string>())
            {
                var equalsIndex = property.IndexOf("=", 1, StringComparison.Ordinal);
                var commaIndex = property.IndexOf(",", 1, StringComparison.Ordinal);
                if (equalsIndex == -1)
                {
                    continue;
                }

                var groupName = property.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1);
                yield return groupName;
            }
        }

        private DirectoryEntry CreateEntry()
        {
            return CreateEntry(_config.Url, _config.UserName, _config.Password);
        }

        private DirectoryEntry CreateEntry(string path)
        {
            return CreateEntry(path, _config.UserName, _config.Password);
        }

        private DirectoryEntry CreateEntry(string userName, string password)
        {
            return CreateEntry(_config.Url, userName, password);
        }

        private DirectoryEntry CreateEntry(string path, string userName, string password)
        {
            var entry = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
            return entry;
        }
    }
}