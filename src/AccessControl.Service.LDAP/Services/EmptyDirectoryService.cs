using System.Collections.Generic;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Service.LDAP.Services
{
    internal class EmptyDirectoryService : ILdapService
    {
        public static readonly ILdapService Instance = new EmptyDirectoryService();

        public bool CheckCredentials(string userName, string password, out IUser user)
        {
            user = null;
            return false;
        }

        public IEnumerable<IDepartment> ListDepartments()
        {
            return Enumerable.Empty<IDepartment>();
        }

        public IEnumerable<IDepartment> FindDepartmentsByManager(string managerName)
        {
            return Enumerable.Empty<IDepartment>();
        }

        public IUser FindUserByName(string userName)
        {
            return null;
        }

        public IEnumerable<IUserGroup> FindUserGroupsByManager(string managerName)
        {
            return Enumerable.Empty<IUserGroup>();
        }

        public IEnumerable<IUser> FindUsersByManager(string managerName)
        {
            return Enumerable.Empty<IUser>();
        }

        public IEnumerable<IUserGroup> GetUserGroups(string userName)
        {
            return Enumerable.Empty<IUserGroup>();
        }

        public IEnumerable<IUser> GetUsersInGroup(string userGroupName)
        {
            return Enumerable.Empty<IUser>();
        }

        public IEnumerable<IUserGroup> ListUserGroups()
        {
            return Enumerable.Empty<IUserGroup>();
        }

        public IEnumerable<IUser> ListUsers()
        {
            return Enumerable.Empty<IUser>();
        }
    }
}