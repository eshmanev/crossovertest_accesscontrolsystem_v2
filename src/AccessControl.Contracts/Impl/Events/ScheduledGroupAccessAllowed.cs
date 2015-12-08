using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class ScheduledGroupAccessAllowed : IScheduledGroupAccessAllowed
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledGroupAccessAllowed" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <param name="usersInGroup">The users in group.</param>
        /// <param name="usersBiometrics">The users biometrics.</param>
        /// <param name="weeklySchedule">The schedule.</param>
        public ScheduledGroupAccessAllowed(Guid accessPointId, string userGroupName, string[] usersInGroup, string[] usersBiometrics, IWeeklySchedule weeklySchedule)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
            Contract.Requires(usersInGroup != null);
            Contract.Requires(usersBiometrics != null);
            Contract.Requires(weeklySchedule != null);

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
            UsersBiometrics = usersBiometrics;
            WeeklySchedule = weeklySchedule;
            UsersInGroup = usersInGroup;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the name of the user group.
        /// </summary>
        /// <value>
        ///     The name of the user group.
        /// </value>
        public string UserGroupName { get; }

        /// <summary>
        ///     Gets the users biometrics.
        /// </summary>
        /// <value>
        ///     The users biometrics.
        /// </value>
        /// <remarks>
        ///     The <see cref="UsersInGroup" /> array correlates the <see cref="UsersBiometrics" /> with indexes.
        /// </remarks>
        public string[] UsersBiometrics { get; }

        /// <summary>
        ///     Gets the users in group.
        /// </summary>
        /// <value>
        ///     The users in group.
        /// </value>
        /// <remarks>
        ///     The <see cref="UsersInGroup" /> array correlates the <see cref="UsersBiometrics" /> with indexes.
        /// </remarks>
        public string[] UsersInGroup { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        public IWeeklySchedule WeeklySchedule { get; }
    }
}