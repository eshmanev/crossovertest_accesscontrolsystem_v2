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

        public static IScheduledAccessRule Scheduled(Guid accessPointId, IWeeklySchedule weeklySchedule)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(weeklySchedule != null);
            return new ScheduledAccessRule(accessPointId, weeklySchedule);
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
            public ScheduledAccessRule(Guid accessPointId, IWeeklySchedule weeklySchedule)
            {
                Contract.Requires(accessPointId != Guid.Empty);
                Contract.Requires(weeklySchedule != null);
                AccessPointId = accessPointId;
                WeeklySchedule = weeklySchedule;
            }

            public Guid AccessPointId { get; }
            public IWeeklySchedule WeeklySchedule { get; }
        }
    }
}