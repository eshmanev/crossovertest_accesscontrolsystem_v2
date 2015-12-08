using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class ScheduledUserAccess : UserPermissionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledUserAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <param name="weeklySchedule">The schedule.</param>
        public ScheduledUserAccess(Guid accessPointId, UserHash userHash, WeeklySchedule weeklySchedule)
            : base(accessPointId, userHash)
        {
            Contract.Requires(weeklySchedule != null);
            WeeklySchedule = weeklySchedule;
        }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        public WeeklySchedule WeeklySchedule { get; }

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        public override bool IsAllowed(Guid accessPointId, string userHash)
        {
            if (AccessPointId != accessPointId)
            {
                return false;
            }

            if (!string.Equals(UserHash.Hash, userHash))
            {
                return false;
            }

            return WeeklySchedule.IsAllowed();
        }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessPermissionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}