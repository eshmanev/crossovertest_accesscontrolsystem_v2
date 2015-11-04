using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.Helpers
{
    public class AllowDenyUserGroupAccess : IAllowUserGroupAccess, IDenyUserGroupAccess
    {
        public AllowDenyUserGroupAccess(Guid accessPointId, string userGroupName)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
        }

        public Guid AccessPointId { get; }

        public string UserGroupName { get; }
    }
}