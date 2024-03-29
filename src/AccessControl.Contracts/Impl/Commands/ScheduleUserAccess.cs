﻿using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class ScheduleUserAccess : IScheduleUserAccess
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduleUserAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="weeklySchedule">The schedule.</param>
        public ScheduleUserAccess(Guid accessPointId, string userName, IWeeklySchedule weeklySchedule)
        {
            Contract.Requires(weeklySchedule != null);
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            AccessPointId = accessPointId;
            UserName = userName;
            WeeklySchedule = weeklySchedule;
        }


        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        public IWeeklySchedule WeeklySchedule { get; }
    }
}