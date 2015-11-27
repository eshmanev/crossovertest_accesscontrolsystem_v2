using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class AccessAttempted : IAccessAttempted
    {
        public AccessAttempted(Guid accessPointId, string userName, bool failed)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            AccessPointId = accessPointId;
            DateTimeCreated = DateTime.Now;
            UserName = userName;
            Failed = failed;
        }

        public Guid AccessPointId { get; }
        public DateTime DateTimeCreated { get; }
        public bool Failed { get; }
        public string UserName { get; }
    }
}