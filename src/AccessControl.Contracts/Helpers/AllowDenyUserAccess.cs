using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.Helpers
{
    public class AllowDenyUserAccess : IAllowUserAccess, IDenyUserAccess
    {
        public AllowDenyUserAccess(Guid accessPointId, string userName)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            AccessPointId = accessPointId;
            UserName = userName;
        }

        public Guid AccessPointId { get; }

        public string UserName { get; }
    }
}