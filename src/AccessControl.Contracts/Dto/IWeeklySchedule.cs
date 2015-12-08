using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IWeeklyScheduleContract))]
    public interface IWeeklySchedule
    {
        /// <summary>
        ///     Gets the time zone for the schedule.
        /// </summary>
        /// <value>
        ///     The time zone.
        /// </value>
        string TimeZone { get; }

        /// <summary>
        ///     Gets the daily time range.
        /// </summary>
        /// <value>
        ///     The daily time range.
        /// </value>
        IDictionary<DayOfWeek, ITimeRange> DailyTimeRange { get; }
    }
}