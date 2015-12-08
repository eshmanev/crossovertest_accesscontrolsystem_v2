using System;

namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a schedule entry.
    /// </summary>
    public class SchedulerEntry
    {
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        /// <value>
        /// The rule.
        /// </value>
        public virtual ScheduledAccessRule Rule { get; internal protected set; }

        /// <summary>
        ///     Gets the day of week.
        /// </summary>
        /// <value>
        ///     The day of week.
        /// </value>
        public virtual DayOfWeek Day { get; set; }

        /// <summary>
        ///     Gets or sets from time in UTC format.
        /// </summary>
        /// <value>
        ///     From time UTC.
        /// </value>
        public virtual TimeSpan FromTime { get; set; }

        /// <summary>
        ///     Gets or sets to time in UTC format.
        /// </summary>
        /// <value>
        ///     To time UTC.
        /// </value>
        public virtual TimeSpan ToTime { get; set; }
    }
}