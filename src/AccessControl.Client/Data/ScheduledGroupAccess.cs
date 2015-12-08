using System;
using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class ScheduledGroupAccess : GroupPermisionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledGroupAccess"/> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <param name="userHashes">The user hashes.</param>
        /// <param name="weeklySchedule">The schedule.</param>
        public ScheduledGroupAccess(Guid accessPointId, string userGroupName, UserHash[] userHashes, IWeeklySchedule weeklySchedule)
            :base(accessPointId, userGroupName, userHashes)
        {
            Contract.Requires(weeklySchedule != null);
            WeeklySchedule = weeklySchedule;
        }

        /// <summary>
        /// Gets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        public IWeeklySchedule WeeklySchedule { get; }

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        public override bool IsAllowed(Guid accessPointId, string userHash)
        {
            return AccessPointId == accessPointId && UserHashes.Any(x => string.Equals(userHash, x.Hash)) && WeeklySchedule.IsAllowed();
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