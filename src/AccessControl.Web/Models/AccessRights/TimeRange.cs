using System;

namespace AccessControl.Web.Models.AccessRights
{
    public class TimeRange
    {
        /// <summary>
        /// Gets or sets from time.
        /// </summary>
        /// <value>
        /// From time.
        /// </value>
        public TimeSpan FromTime { get; set; }

        /// <summary>
        /// Gets or sets to time.
        /// </summary>
        /// <value>
        /// To time.
        /// </value>
        public TimeSpan ToTime { get; set; }
    }
}