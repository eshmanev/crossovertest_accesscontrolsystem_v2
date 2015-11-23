using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    /// <summary>
    ///     Represents a simple list command.
    /// </summary>
    public static class ListCommand
    {
        public static readonly Command Default = new Command();

        public static IListUsersInGroup ListUsersInGroup(string userGroupName)
        {
            return new ListUsersInGroupImpl {UserGroupName = userGroupName};
        }

        #region Commands

        public class Command : IListUsers, IListAccessPoints, IListAccessRights, IListDepartments, IListUserGroups, IListUsersBiometric
        {
        }

        private class ListUsersInGroupImpl : IListUsersInGroup
        {
            public string UserGroupName { get; set; }
        }

        #endregion

        #region Results

        public static IListAccessPointsResult AccessPointsResult(IAccessPoint[] accessPoints)
        {
            Contract.Requires(accessPoints != null);
            return new ListAccessPointsResultImpl {AccessPoints = accessPoints};
        }

        public static IListUsersResult UsersResult(IUser[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersResultImpl {Users = users};
        }

        public static IListUsersInGroupResult UsersInGroupResult(IUser[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersInGroupResultImpl { Users = users };
        }

        public static IListDepartmentsResult DepartmentsResult(IDepartment[] departments)
        {
            Contract.Requires(departments != null);
            return new ListDepartmentsResultImpl {Departments = departments};
        }

        public static IListUserGroupsResult UserGroupsResult(IUserGroup[] groups)
        {
            Contract.Requires(groups != null);
            return new ListUserGroupsResultImpl {Groups = groups};
        }

        public static IListUsersBiometricResult UsersBiometricResult(IUserBiometric[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersBiometricResultImpl {Users = users};
        }

        public static IListAccessRightsResult AccessRightsResult(IUserAccessRights[] userAccessRights, IUserGroupAccessRights[] userGroupAccessRights)
        {
            Contract.Requires(userAccessRights != null);
            Contract.Requires(userGroupAccessRights != null);
            return new ListAccessRightsResultImpl {UserAccessRights = userAccessRights, UserGroupAccessRights = userGroupAccessRights};
        }

        #endregion

        #region Nested classes

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