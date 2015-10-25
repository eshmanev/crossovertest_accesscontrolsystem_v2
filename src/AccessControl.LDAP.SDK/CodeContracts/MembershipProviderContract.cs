using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace AccessControl.LDAP.SDK.CodeContracts
{
    [ContractClassFor(typeof(IMembershipProvider))]
    internal class MembershipProviderContract : IMembershipProvider
    {
        Manager IMembershipProvider.GetManager(string siteLocation, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteLocation));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Ensures(Contract.Result<Manager>() != null);
            return null;
        }

        User IMembershipProvider.ValidateUser(string userName, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            return null;
        }

        UserGroup IMembershipProvider.GetUserGroup(string siteLocation, string department, string userGroupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteLocation));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
            Contract.Ensures(Contract.Result<UserGroup>() != null);
            return null;
        }

        UserDepartment IMembershipProvider.GetUserDepartment(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Ensures(Contract.Result<UserDepartment>() != null);
            return null;
        }

        IEnumerable<User> IMembershipProvider.GetUsersInGroup(string siteLocation, string department, string userGroupName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteLocation));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
            Contract.Ensures(Contract.Result<IEnumerable<User>>() != null);
            return null;
        }

        IEnumerable<Site> IMembershipProvider.GetSites()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Site>>() != null);
            return null;
        }

        IEnumerable<Department> IMembershipProvider.GetDepartments(string siteLocation)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteLocation));
            Contract.Ensures(Contract.Result<IEnumerable<Department>>() != null);
            return null;
        }

        IEnumerable<UserGroup> IMembershipProvider.GetUserGroups(string siteLocation, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteLocation));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Ensures(Contract.Result<IEnumerable<UserGroup>>() != null);
            return null;
        }

        User IMembershipProvider.GetUser(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Ensures(Contract.Result<User>() != null);
            return null;
        }

        IEnumerable<User> IMembershipProvider.GetUsers(string siteLocation, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteLocation));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Ensures(Contract.Result<IEnumerable<User>>() != null);
            return null;
        }
    }
}