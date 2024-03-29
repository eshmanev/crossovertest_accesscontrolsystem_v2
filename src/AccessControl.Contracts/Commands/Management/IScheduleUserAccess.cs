﻿using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Creates a weekly schedule for the specified user and access point.
    /// </summary>
    [ContractClass(typeof(IScheduleUserAccessContract))]
    public interface IScheduleUserAccess
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user name.
        /// </summary>
        string UserName { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        IWeeklySchedule WeeklySchedule { get; }
    }
}