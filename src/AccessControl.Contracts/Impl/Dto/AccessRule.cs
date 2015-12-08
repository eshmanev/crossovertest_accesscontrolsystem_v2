using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public abstract class AccessRule
    {
        public static IPermanentAccessRule Permanent(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            return new PermanentAccessRule(accessPointId);
        }

        public static IScheduledAccessRule Scheduled(Guid accessPointId, ISchedule schedule)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(schedule != null);
            return new ScheduledAccessRule(accessPointId, schedule);
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
            public ScheduledAccessRule(Guid accessPointId, ISchedule schedule)
            {
                Contract.Requires(accessPointId != Guid.Empty);
                Contract.Requires(schedule != null);
                AccessPointId = accessPointId;
                Schedule = schedule;
            }

            public Guid AccessPointId { get; }
            public ISchedule Schedule { get; }
        }
    }
}