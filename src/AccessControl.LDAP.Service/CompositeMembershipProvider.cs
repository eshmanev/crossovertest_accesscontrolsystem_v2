using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.LDAP.SDK;

namespace AccessControl.LDAP.Service
{
    internal class CompositeMembershipProvider : IMembershipProvider
    {
        private readonly IMembershipProvider _provider;

        public CompositeMembershipProvider(IMembershipProvider provider)
        {
            Contract.Requires(provider != null);
            _provider = provider;
        }

        public IEnumerable<Site> GetSites()
        {
            return _provider.GetSites();
        }

        public IEnumerable<Department> GetDepartments(string siteLocation)
        {
            return _provider.GetDepartments(siteLocation);
        }

        public IEnumerable<User> GetUsers(string siteLocation, string department)
        {
            return _provider.GetUsers(siteLocation, department);
        }

        public IEnumerable<UserGroup> GetUserGroups(string siteLocation, string department)
        {
            return _provider.GetUserGroups(siteLocation, department);
        }

        public Manager GetManager(string siteLocation, string department)
        {
            return _provider.GetManager(siteLocation, department);
        }

        public User ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public User GetUser(string userName)
        {
            return _provider.GetUser(userName);
        }

        public UserGroup GetUserGroup(string siteLocation, string department, string userGroupName)
        {
            return _provider.GetUserGroup(siteLocation, department, userGroupName);
        }

        public UserDepartment GetUserDepartment(string userName)
        {
            return _provider.GetUserDepartment(userName);
        }

        public IEnumerable<User> GetUsersInGroup(string siteLocation, string department, string userGroupName)
        {
            return _provider.GetUsersInGroup(siteLocation, department, userGroupName);
        }
    }
}