using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class WeeklySchedule : IWeeklySchedule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WeeklySchedule" /> class.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        public WeeklySchedule(string timeZone)
        {
            Contract.Ensures(TimeZoneInfo.FindSystemTimeZoneById(timeZone) != null);
            TimeZone = timeZone;
            DailyTimeRange = new Dictionary<DayOfWeek, ITimeRange>();
        }

        /// <summary>
        ///     Gets the time zone for the schedule.
        /// </summary>
        /// <value>
        ///     The time zone.
        /// </value>
        public string TimeZone { get; }

        /// <summary>
        ///     Gets the daily time range.
        /// </summary>
        /// <value>
        ///     The daily time range.
        /// </value>
        public IDictionary<DayOfWeek, ITimeRange> DailyTimeRange { get; }
    }
}