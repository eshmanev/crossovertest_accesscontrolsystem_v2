using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Linq;
using AccessControl.Contracts.Dto;
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
            var userResult = FindUserByNameCore(userName);
            if (userResult == null)
            {
                return Enumerable.Empty<IUserGroup>();
            }

            using (var entry = CreateEntry())
            using (var searcher = new DirectorySearcher(entry))
            {
                var groups = new List<IUserGroup>();
                foreach (var group in userResult.Properties["memberOf"].OfType<string>())
                {
                    searcher.Filter = $"(&(objectClass=group)(distinguishedName={group}))";
                    var groupResult = searcher.FindOne();
                    if (groupResult != null)
                    {
                        groups.Add(ConvertUserGroup(groupResult));
                    }
                }
                return groups;
            }
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
                    searcher.Filter = $"(&(objectClass=group)(name={FromUniqueName(userGroupName)}))";
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
                return Enumerable.Empty<IUser>();

            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(&(objectClass=user)(manager={manager.GetProperty("distinguishedname")}))"})
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
        ///     Finds the user groups by manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        public IEnumerable<IUserGroup> FindUserGroupsByManager(string managerName)
        {
            var manager = FindUserByNameCore(managerName);
            if (manager == null)
                return Enumerable.Empty<IUserGroup>();

            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = $"(&(objectClass=group)(managedby={manager.GetProperty("distinguishedname")}))";
                    searcher.SearchScope = SearchScope.Subtree;

                    return searcher.FindAll().Cast<SearchResult>()
                                   .Select(ConvertUserGroup)
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
                                   .Select(ConvertUserGroup)
                                   .ToArray();
                }
            }
        }

        /// <summary>
        ///     Checks the specified credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns></returns>
        public bool CheckCredentials(string userName, string password, out IUser user)
        {
            try
            {
                using (var entry = new DirectoryEntry(_config.Url, userName, password))
                {
                    entry.AuthenticationType = AuthenticationTypes.Secure;
                    // ReSharper disable once UnusedVariable
                    var nativeObject = entry.NativeObject;
                    using (var searcher = new DirectorySearcher(entry) {Filter = $"(sAMAccountName={FromUniqueName(userName)})"})
                    {
                        var result = searcher.FindOne();
                        user = result != null ? ConvertUser(result) : null;
                    }

                    return user != null;
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
                return Enumerable.Empty<IDepartment>();

            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(&(objectClass=user)(manager={manager.GetProperty("distinguishedname")}))"})
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

        private IUserGroup ConvertUserGroup(SearchResult result)
        {
            return new UserGroup(ToUniqueName(result.GetProperty("name")), result.GetProperty("name"));
        }

        private IDepartment ConvertDepartment(SearchResult result)
        {
            var department = result.GetProperty("department");
            return new Department(department);
        }

        private IUser ConvertUser(SearchResult result)
        {
            var userName = ToUniqueName(result.GetProperty("sAMAccountName"));
            var managerName = result.GetProperty("manager");
            var manager = managerName != null ? FindUserByDistinguishedNameCore(managerName) : null;
            return new User(userName, GetUserGroups(userName).ToArray())
            {
                DisplayName = result.GetProperty("displayname") ?? userName,
                PhoneNumber = result.GetProperty("telephonenumber"),
                Email = result.GetProperty("mail"),
                Department = result.GetProperty("department"),
                IsManager = FindUsersByManager(userName).Any(),
                ManagerName = ToUniqueName(manager?.GetProperty("sAMAccountName"))
            };
        }

        private SearchResult FindUserByNameCore(string userName)
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) {Filter = $"(sAMAccountName={FromUniqueName(userName)})"})
                {
                    return searcher.FindOne();
                }
            }
        }

        private SearchResult FindUserByDistinguishedNameCore(string userName)
        {
            using (var entry = CreateEntry())
            {
                using (var searcher = new DirectorySearcher(entry) { Filter = $"(distinguishedName={userName})" })
                {
                    return searcher.FindOne();
                }
            }
        }

        private DirectoryEntry CreateEntry()
        {
            return CreateEntry(_config.Url, _config.UserName, _config.Password);
        }

        private DirectoryEntry CreateEntry(string path, string userName, string password)
        {
            var entry = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
            return entry;
        }

        private string ToUniqueName(string name)
        {
            if (name == null)
                return null;

            return $"{_config.DomainName}\\{name}";
        }

        private string FromUniqueName(string name)
        {
            if (name == null)
                return null;

            var parts = name.Split('\\');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid unique name specified");

            return parts[1];
        }
    }
}