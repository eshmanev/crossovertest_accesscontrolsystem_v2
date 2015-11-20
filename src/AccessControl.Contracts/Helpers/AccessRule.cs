using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public abstract class AccessRule
    {
        public static IPermanentAccessRule Permanent(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            return new PermanentAccessRule(accessPointId);
        }

        public static IScheduledAccessRule Scheduled(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            return new ScheduledAccessRule(accessPointId);
        }

        private class PermanentAccessRule : AccessRule, IPermanentAccessRule
        {
            public PermanentAccessRule(Guid accessPointId)
            {
                Contract.Requires(accessPointId != Guid.Empty);
                AccessPointId = accessPointId;
            }

            public Guid AccessPointId { get; }
        }

        private class ScheduledAccessRule : AccessRule, IScheduledAccessRule
        {
            public ScheduledAccessRule(Guid accessPointId)
            {
                Contract.Requires(accessPointId != Guid.Empty);
                AccessPointId = accessPointId;
            }

            public Guid AccessPointId { get; }
        }
    }
}