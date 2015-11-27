using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class PermanentUserGroupAccessAllowed : IPermanentUserGroupAccessAllowed
    {
        public PermanentUserGroupAccessAllowed(Guid accessPointId, string userGroupName, string[] usersInGroup, string[] usersBiometrics)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
            Contract.Requires(usersInGroup != null);
            Contract.Requires(usersBiometrics != null);

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
            UsersBiometrics = usersBiometrics;
            UsersInGroup = usersInGroup;
        }

        public Guid AccessPointId { get; }
        public string UserGroupName { get; }
        public string[] UsersBiometrics { get; }
        public string[] UsersInGroup { get; }
    }
}