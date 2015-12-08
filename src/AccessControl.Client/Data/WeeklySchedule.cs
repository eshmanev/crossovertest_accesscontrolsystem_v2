using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using AccessControl.Contracts.Dto;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class WeeklySchedule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WeeklySchedule" /> class.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <param name="entries">The entries.</param>
        [Pure]
        public WeeklySchedule(string timeZone, WeeklyScheduleEntry[] entries)
        {
            Contract.Requires(TimeZoneInfo.FindSystemTimeZoneById(timeZone) != null);
            Contract.Requires(entries != null);
            TimeZone = timeZone;
            Entries = entries;
        }

        /// <summary>
        ///     Gets the time zone.
        /// </summary>
        /// <value>
        ///     The time zone.
        /// </value>
        public string TimeZone { get; }

        /// <summary>
        ///     Gets the entries.
        /// </summary>
        /// <value>
        ///     The entries.
        /// </value>
        public WeeklyScheduleEntry[] Entries { get; }

        /// <summary>
        ///     Determines whether the scheduler allows access at the current time.
        /// </summary>
        /// <returns></returns>
        public bool IsAllowed()
        {
            var now = DateTime.Now;
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
            return Entries.Any(
                x =>
                {
                    var localFromTime = TimeZoneInfo.ConvertTime(new DateTime(x.FromTime.Ticks), timeZoneInfo, TimeZoneInfo.Local).TimeOfDay;
                    var localToTime = TimeZoneInfo.ConvertTime(new DateTime(x.ToTime.Ticks), timeZoneInfo, TimeZoneInfo.Local).TimeOfDay;
                    return now.DayOfWeek == x.DayOfWeek && now.TimeOfDay >= localFromTime && now.TimeOfDay <= localToTime;
                });
        }

        /// <summary>
        ///     Converts the specified schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static WeeklySchedule Convert(IWeeklySchedule schedule)
        {
            var entries = schedule.DailyTimeRange.Select(x => new WeeklyScheduleEntry(x.Key, x.Value.FromTime, x.Value.ToTime)).ToArray();
            return new WeeklySchedule(schedule.TimeZone, entries);
        }
    }
}