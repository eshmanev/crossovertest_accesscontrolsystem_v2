﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;
using AccessControl.Service.LDAP.CodeContracts;

namespace AccessControl.Service.LDAP.Services
{
    [ContractClass(typeof(ILdapServiceContract))]
    internal interface ILdapService
    {
        /// <summary>
        ///     Authenticates the specified user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        bool Authenticate(string userName, string password);

        /// <summary>
        ///     Finds the departments managed by the given manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        IEnumerable<IDepartment> FindDepartmentsByManager(string managerName);

        /// <summary>
        ///     Finds the user by name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        IUser FindUserByName(string userName);

        /// <summary>
        ///     Finds the user groups managed by the specified manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        IEnumerable<IUserGroup> FindUserGroupsByManager(string managerName);

        /// <summary>
        ///     Finds the users managed by the given manager.
        /// </summary>
        /// <param name="managerName">Name of the manager.</param>
        /// <returns></returns>
        IEnumerable<IUser> FindUsersByManager(string managerName);

        /// <summary>
        ///     Gets the user groups in which the specified user is member of.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        IEnumerable<IUserGroup> GetUserGroups(string userName);

        /// <summary>
        ///     Gets the users in the specified group.
        /// </summary>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <returns></returns>
        IEnumerable<IUser> GetUsersInGroup(string userGroupName);

        /// <summary>
        ///     Lists the user groups.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IUserGroup> ListUserGroups();

        /// <summary>
        ///     Lists the users.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IUser> ListUsers();

        /// <summary>
        ///     Check if the specified department exists..
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="department">The department.</param>
        /// <returns></returns>
        bool ValidateDepartment(string site, string department);
    }
}