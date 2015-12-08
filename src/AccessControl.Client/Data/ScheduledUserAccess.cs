using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class ScheduledUserAccess : IAccessPermission
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledUserAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <param name="sourceFromTime">From time UTC.</param>
        /// <param name="sourceToTime">To time UTC.</param>
        /// <param name="sourceTimeZone">The source time zone.</param>
        public ScheduledUserAccess(Guid accessPointId, UserHash userHash, DayOfWeek dayOfWeek, TimeSpan sourceFromTime, TimeSpan sourceToTime, string sourceTimeZone)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(sourceTimeZone != null);

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZone);
            AccessPointId = accessPointId;
            UserHash = userHash;
            DayOfWeek = dayOfWeek;
            SourceFromTime = sourceFromTime;
            SourceToTime = sourceToTime;
            SourceTimeZone = sourceTimeZone;

            LocalFromTime = TimeZoneInfo.ConvertTime(new DateTime(sourceFromTime.Ticks), timeZoneInfo, TimeZoneInfo.Local).TimeOfDay;
            LocalToTime = TimeZoneInfo.ConvertTime(new DateTime(sourceToTime.Ticks), timeZoneInfo, TimeZoneInfo.Local).TimeOfDay;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets From time.
        /// </summary>
        /// <value>
        ///     From time.
        /// </value>
        public TimeSpan SourceFromTime { get; }

        /// <summary>
        /// Gets the local From time.
        /// </summary>
        /// <value>
        /// The local from time.
        /// </value>
        public TimeSpan LocalFromTime { get; private set; }

        /// <summary>
        ///     Gets to time.
        /// </summary>
        /// <value>
        ///     To time.
        /// </value>
        public TimeSpan SourceToTime { get; }

        /// <summary>
        /// Gets the local TO time.
        /// </summary>
        /// <value>
        /// The local to time.
        /// </value>
        public TimeSpan LocalToTime { get; private set; }

        /// <summary>
        ///     Gets or sets the time zone.
        /// </summary>
        /// <value>
        ///     The time zone.
        /// </value>
        public string SourceTimeZone { get; set; }

        /// <summary>
        ///     Gets the user hash.
        /// </summary>
        /// <value>
        ///     The user hash.
        /// </value>
        public UserHash UserHash { get; }

        /// <summary>
        ///     Gets the day of week.
        /// </summary>
        /// <value>
        ///     The day of week.
        /// </value>
        public DayOfWeek DayOfWeek { get; }

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        public bool IsAllowed(Guid accessPointId, string userHash)
        {
            if (AccessPointId != accessPointId)
            {
                return false;
            }

            if (!string.Equals(UserHash.Hash, userHash))
            {
                return false;
            }
            
            var now = DateTime.Now;
            return now.DayOfWeek == DayOfWeek && now.TimeOfDay >= LocalFromTime && now.TimeOfDay <= LocalToTime;
        }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IAccessPermissionVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as ScheduledUserAccess;
            return other != null && other.AccessPointId == AccessPointId && string.Equals(other.UserHash.UserName, UserHash.UserName);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return "ScheduledUserAccess".GetHashCode() ^ AccessPointId.GetHashCode() ^ UserHash.UserName.GetHashCode();
        }
    }
}