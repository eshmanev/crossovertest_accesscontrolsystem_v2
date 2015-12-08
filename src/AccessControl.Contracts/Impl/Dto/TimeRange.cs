using System;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class TimeRange : ITimeRange
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TimeRange" /> class.
        /// </summary>
        /// <param name="fromTime">From time UTC.</param>
        /// <param name="toTime">To time UTC.</param>
        public TimeRange(TimeSpan fromTime, TimeSpan toTime)
        {
            FromTime = fromTime;
            ToTime = toTime;
        }

        /// <summary>
        ///     Gets FROM time, in UTC.
        /// </summary>
        /// <value>
        ///     FROM time, UTC.
        /// </value>
        public TimeSpan FromTime { get; }

        /// <summary>
        ///     Gets TO time, in UTC.
        /// </summary>
        /// <value>
        ///     TO time, UTC.
        /// </value>
        public TimeSpan ToTime { get; }
    }
}