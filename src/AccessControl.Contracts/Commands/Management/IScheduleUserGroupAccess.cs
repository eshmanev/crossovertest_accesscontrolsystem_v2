using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Management
{
    [ContractClass(typeof(IScheduleUserGroupAccessContract))]
    public interface IScheduleUserGroupAccess
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user group name.
        /// </summary>
        string UserGroupName { get; }

        /// <summary>
        ///     Gets the schedule.
        /// </summary>
        /// <value>
        ///     The schedule.
        /// </value>
        IWeeklySchedule WeeklySchedule { get; }
    }
}