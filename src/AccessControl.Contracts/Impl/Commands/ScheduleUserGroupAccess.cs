﻿using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class ScheduleUserGroupAccess : IScheduleUserGroupAccess
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduleUserAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <param name="weeklySchedule">The schedule.</param>
        public ScheduleUserGroupAccess(Guid accessPointId, string userGroupName, IWeeklySchedule weeklySchedule)
        {
            Contract.Requires(weeklySchedule != null);
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
            WeeklySchedule = weeklySchedule;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user group name.
        /// </summary>
        public string UserGroupName { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        public IWeeklySchedule WeeklySchedule { get; }
    }
}