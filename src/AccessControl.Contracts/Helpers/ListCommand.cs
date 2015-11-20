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

        #region Commands

        public class Command : IListUsers, IListAccessPoints, IListAccessRights, IListDepartments, IListUserGroups, IListUsersBiometric
        {
        }

        #endregion

        #region Results

        public static IListAccessPointsResult Result(IAccessPoint[] accessPoints)
        {
            Contract.Requires(accessPoints != null);
            return new ListAccessPointsResult {AccessPoints = accessPoints};
        }

        public static IListUsersResult Result(IUser[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersResult {Users = users};
        }

        public static IListDepartmentsResult Result(IDepartment[] departments)
        {
            Contract.Requires(departments != null);
            return new ListDepartmentsResult {Departments = departments};
        }

        public static IListUserGroupsResult Result(IUserGroup[] groups)
        {
            Contract.Requires(groups != null);
            return new ListUserGroupsResult {Groups = groups};
        }

        public static IListUsersBiometricResult Result(IUserBiometric[] users)
        {
            Contract.Requires(users != null);
            return new ListUsersBiometricResult {Users = users};
        }

        public static IListAccessRightsResult Result(IUserAccessRights[] userAccessRights, IUserGroupAccessRights[] userGroupAccessRights)
        {
            Contract.Requires(userAccessRights != null);
            Contract.Requires(userGroupAccessRights != null);
            return new ListAccessRightsResult {UserAccessRights = userAccessRights, UserGroupAccessRights = userGroupAccessRights};
        }

        #endregion

        #region Nested classes

        private class ListAccessPointsResult : IListAccessPointsResult
        {
            public IAccessPoint[] AccessPoints { get; set; }
        }

        private class ListUsersResult : IListUsersResult
        {
            public IUser[] Users { get; set; }
        }

        private class ListDepartmentsResult : IListDepartmentsResult
        {
            public IDepartment[] Departments { get; set; }
        }

        private class ListUserGroupsResult : IListUserGroupsResult
        {
            public IUserGroup[] Groups { get; set; }
        }

        private class ListUsersBiometricResult : IListUsersBiometricResult
        {
            public IUserBiometric[] Users { get; set; }
        }

        private class ListAccessRightsResult : IListAccessRightsResult
        {
            public IUserAccessRights[] UserAccessRights { get; set; }
            public IUserGroupAccessRights[] UserGroupAccessRights { get; set; }
        }

        #endregion
    }
}