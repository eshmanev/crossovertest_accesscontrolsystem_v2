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

        public static IScheduledAccessRule Scheduled(Guid accessPointId, TimeSpan fromTimeUtc, TimeSpan toTimeUtc)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            return new ScheduledAccessRule(accessPointId, fromTimeUtc, toTimeUtc);
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
            public ScheduledAccessRule(Guid accessPointId, TimeSpan fromTimeUtc, TimeSpan toTimeUtc)
            {
                Contract.Requires(accessPointId != Guid.Empty);
                AccessPointId = accessPointId;
                FromTimeUtc = fromTimeUtc;
                ToTimeUtc = toTimeUtc;
            }

            public Guid AccessPointId { get; }
            public TimeSpan FromTimeUtc { get; }
            public TimeSpan ToTimeUtc { get; }
        }
    }
}