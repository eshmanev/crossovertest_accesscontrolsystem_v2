using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.LDAP.SDK.CodeContracts;

namespace AccessControl.LDAP.SDK
{
    [ContractClass(typeof(MembershipProviderContract))]
    public interface IMembershipProvider
    {
        /// <summary>
        /// Gets a collection of sites.
        /// </summary>
        /// <returns>A collection of the <see cref="Site"/> objects.</returns>
        IEnumerable<Site> GetSites();

        /// <summary>
        /// Gets a collection of departments at the specified site location.
        /// </summary>
        /// <returns>A collection of the <see cref="Department"/> objects.</returns>
        IEnumerable<Department> GetDepartments(string siteLocation);

        /// <summary>
        /// Returns a list of users for the specified department.
        /// </summary>
        /// <param name="siteLocation">The site location.</param>
        /// <param name="department">The department.</param>
        /// <returns>A list of <see cref="User"/> objects.</returns>
        IEnumerable<User> GetUsers(string siteLocation, string department);

        /// <summary>
        /// Returns a list of user groups for the specified department.
        /// </summary>
        /// <param name="siteLocation">The site location.</param>
        /// <param name="department">The department.</param>
        /// <returns>A list of <see cref="UserGroup"/> objects.</returns>
        IEnumerable<UserGroup> GetUserGroups(string siteLocation, string department);

        /// <summary>
        /// Gets the manager of the specified department.
        /// </summary>
        /// <param name="siteLocation">The site location.</param>
        /// <param name="department">The department.</param>
        /// <returns>An instance of the <see cref="Manager"/>.</returns>
        Manager GetManager(string siteLocation, string department);

        /// <summary>
        /// Validates the user's credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>An instance of the <see cref="User"/> class if credentials are valid; otherwise, null.</returns>
        User ValidateUser(string userName, string password);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>An instance of the <see cref="User"/> class.</returns>
        User GetUser(string userName);

        /// <summary>
        /// Gets the user group.
        /// </summary>
        /// <param name="siteLocation">The site location.</param>
        /// <param name="department">The department.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <returns>An instance of the <see cref="UserGroup"/> class.</returns>
        UserGroup GetUserGroup(string siteLocation, string department, string userGroupName);

        /// <summary>
        /// Gets the department which employs the specified user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        UserDepartment GetUserDepartment(string userName);

        /// <summary>
        /// Returns a list of users in specified user group.
        /// </summary>
        /// <param name="siteLocation">The site location.</param>
        /// <param name="department">The department.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <returns>A list of <see cref="User"/> objects.</returns>
        IEnumerable<User> GetUsersInGroup(string siteLocation, string department, string userGroupName);
    }
}