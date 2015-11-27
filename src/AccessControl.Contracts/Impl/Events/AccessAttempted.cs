using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class AccessAttempted : IAccessAttempted
    {
        public AccessAttempted(Guid accessPointId, string biometricHash, bool failed)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(biometricHash));
            AccessPointId = accessPointId;
            CreatedUtc = DateTime.UtcNow;
            BiometricHash = biometricHash;
            Failed = failed;
        }

        public Guid AccessPointId { get; }
        public DateTime CreatedUtc { get; }
        public bool Failed { get; }
        public string BiometricHash { get; }
    }
}