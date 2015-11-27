using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    /// <summary>
    ///     Represents a simple list command.
    /// </summary>
    public static class ListCommand
    {
        /// <summary>
        ///     The default command without parameters.
        /// </summary>
        public static readonly Command WithoutParameters = new Command();

        public static IListLogs ListLogs(DateTime fromDateUtc, DateTime toDateUtc)
        {
            return new ListLogsImpl {FromDateUtc = fromDateUtc, ToDateUtc = toDateUtc};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListUsersInGroup" /> command.
        /// </summary>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <returns></returns>
        public static IListUsersInGroup ListUsersInGroup(string userGroupName)
        {
            return new ListUsersInGroupImpl {UserGroupName = userGroupName};
        }

        #region Commands

        public class Command : IListUsers, IListAccessPoints, IListAccessRights, IListDepartments, IListUserGroups, IListUsersBiometric, IListDelegatedUsers
        {
        }

        private class ListUsersInGroupImpl : IListUsersInGroup
        {
            public string UserGroupName { get; set; }
        }

        private class ListLogsImpl : IListLogs
        {
            public DateTime FromDateUtc { get; set; }
            public DateTime ToDateUtc { get; set; }
        }

        #endregion

        #region Results

        /// <summary>
        ///     Creates a new instance of the <see cref="IListAccessPointsResult" />.
        /// </summary>
        /// <param name="accessPoints">The access points.</param>
        /// <returns></returns>
        public static IListAccessPointsResult AccessPointsResult(IAccessPoint[] accessPoints)
        {
            Contract.Requires(accessPoints != null);
            return new ListAccessPointsResultImpl {AccessPoints = accessPoints};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListUsersResult" />.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public static IListUsersResult UsersResult(IUser[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersResultImpl {Users = users};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListUsersInGroupResult" />.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public static IListUsersInGroupResult UsersInGroupResult(IUser[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersInGroupResultImpl {Users = users};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListDepartmentsResult" />.
        /// </summary>
        /// <param name="departments">The departments.</param>
        /// <returns></returns>
        public static IListDepartmentsResult DepartmentsResult(IDepartment[] departments)
        {
            Contract.Requires(departments != null);
            return new ListDepartmentsResultImpl {Departments = departments};
        }

        /// <summary>
        ///     ss
        ///     Creates a new instance of the <see cref="IListUserGroupsResult" />.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public static IListUserGroupsResult UserGroupsResult(IUserGroup[] groups)
        {
            Contract.Requires(groups != null);
            return new ListUserGroupsResultImpl {Groups = groups};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListUsersBiometricResult" />.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public static IListUsersBiometricResult UsersBiometricResult(IUserBiometric[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersBiometricResultImpl {Users = users};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListAccessRightsResult" />.
        /// </summary>
        /// <param name="userAccessRights">The user access rights.</param>
        /// <param name="userGroupAccessRights">The user group access rights.</param>
        /// <returns></returns>
        public static IListAccessRightsResult AccessRightsResult(IUserAccessRights[] userAccessRights, IUserGroupAccessRights[] userGroupAccessRights)
        {
            Contract.Requires(userAccessRights != null);
            Contract.Requires(userGroupAccessRights != null);
            return new ListAccessRightsResultImpl {UserAccessRights = userAccessRights, UserGroupAccessRights = userGroupAccessRights};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListLogsResult" />.
        /// </summary>
        /// <param name="logs">The logs.</param>
        /// <returns></returns>
        public static IListLogsResult LogsResult(ILogEntry[] logs)
        {
            Contract.Requires(logs != null);
            return new ListLogsResultImpl {Logs = logs};
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="IListDelegatedUsersResult" />.
        /// </summary>
        /// <param name="userNames">The user names.</param>
        /// <returns></returns>
        public static IListDelegatedUsersResult DelegatedUsersResult(string[] userNames)
        {
            Contract.Requires(userNames != null);
            return new ListDelegatedUsersResultImpl { UserNames = userNames };
        }

        #endregion

        #region Nested classes

        private class ListDelegatedUsersResultImpl : IListDelegatedUsersResult
        {
            public string[] UserNames { get; set; }
        }

        private class ListLogsResultImpl : IListLogsResult
        {
            public ILogEntry[] Logs { get; set; }
        }

        private class ListAccessPointsResultImpl : IListAccessPointsResult
        {
            public IAccessPoint[] AccessPoints { get; set; }
        }

        private class ListUsersResultImpl : IListUsersResult
        {
            public IUser[] Users { get; set; }
        }

        private class ListUsersInGroupResultImpl : IListUsersInGroupResult
        {
            public IUser[] Users { get; set; }
        }

        private class ListDepartmentsResultImpl : IListDepartmentsResult
        {
            public IDepartment[] Departments { get; set; }
        }

        private class ListUserGroupsResultImpl : IListUserGroupsResult
        {
            public IUserGroup[] Groups { get; set; }
        }

        private class ListUsersBiometricResultImpl : IListUsersBiometricResult
        {
            public IUserBiometric[] Users { get; set; }
        }

        private class ListAccessRightsResultImpl : IListAccessRightsResult
        {
            public IUserAccessRights[] UserAccessRights { get; set; }
            public IUserGroupAccessRights[] UserGroupAccessRights { get; set; }
        }

        #endregion
    }
}