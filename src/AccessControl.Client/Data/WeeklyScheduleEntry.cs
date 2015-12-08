using System;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class WeeklyScheduleEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WeeklyScheduleEntry" /> class.
        /// </summary>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <param name="fromTime">From time.</param>
        /// <param name="toTime">To time.</param>
        public WeeklyScheduleEntry(DayOfWeek dayOfWeek, TimeSpan fromTime, TimeSpan toTime)
        {
            DayOfWeek = dayOfWeek;
            FromTime = fromTime;
            ToTime = toTime;
        }

        /// <summary>
        ///     Gets the day of week.
        /// </summary>
        /// <value>
        ///     The day of week.
        /// </value>
        public DayOfWeek DayOfWeek { get; }

        /// <summary>
        ///     Gets the From time.
        /// </summary>
        /// <value>
        ///     From time.
        /// </value>
        public TimeSpan FromTime { get; }

        /// <summary>
        ///     Gets the To time.
        /// </summary>
        /// <value>
        ///     To time.
        /// </value>
        public TimeSpan ToTime { get; }
    }
}