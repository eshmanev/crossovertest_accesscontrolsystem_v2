using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public abstract class AccessRights
    {
        public static IUserAccessRights ForUser(string userName, IPermanentAccessRule[] permanentAccessRules, IScheduledAccessRule[] scheduledAccessRules)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(permanentAccessRules != null);
            Contract.Requires(scheduledAccessRules != null);
            return new UserAccessRights(userName, permanentAccessRules, scheduledAccessRules);
        }

        public static IUserGroupAccessRights ForUserGroup(string userGroupName, IPermanentAccessRule[] permanentAccessRules, IScheduledAccessRule[] scheduledAccessRules)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
            Contract.Requires(permanentAccessRules != null);
            Contract.Requires(scheduledAccessRules != null);
            return new UserGroupAccessRights(userGroupName, permanentAccessRules, scheduledAccessRules);
        }

        private class UserGroupAccessRights : AccessRights, IUserGroupAccessRights
        {
            public UserGroupAccessRights(string userGroupName, IPermanentAccessRule[] permanentAccessRules, IScheduledAccessRule[] scheduledAccessRules)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
                Contract.Requires(permanentAccessRules != null);
                Contract.Requires(scheduledAccessRules != null);

                UserGroupName = userGroupName;
                PermanentAccessRules = permanentAccessRules;
                ScheduledAccessRules = scheduledAccessRules;
            }

            public string UserGroupName { get; }

            public IPermanentAccessRule[] PermanentAccessRules { get; }
            public IScheduledAccessRule[] ScheduledAccessRules { get; }
        }

        private class UserAccessRights : AccessRights, IUserAccessRights
        {
            public UserAccessRights(string userName, IPermanentAccessRule[] permanentAccessRules, IScheduledAccessRule[] scheduledAccessRules)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(userName));
                Contract.Requires(permanentAccessRules != null);
                Contract.Requires(scheduledAccessRules != null);

                UserName = userName;
                PermanentAccessRules = permanentAccessRules;
                ScheduledAccessRules = scheduledAccessRules;
            }

            public string UserName { get; }
            public IPermanentAccessRule[] PermanentAccessRules { get; }
            public IScheduledAccessRule[] ScheduledAccessRules { get; }
        }
    }
}