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
        /// <param name="fromTimeUtc">From time UTC.</param>
        /// <param name="toTimeUtc">To time UTC.</param>
        public ScheduledUserAccess(Guid accessPointId, UserHash userHash, TimeSpan fromTimeUtc, TimeSpan toTimeUtc)
        {
            Contract.Requires(accessPointId != Guid.Empty);

            AccessPointId = accessPointId;
            UserHash = userHash;
            FromTimeUtc = fromTimeUtc;
            ToTimeUtc = toTimeUtc;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets from time UTC.
        /// </summary>
        /// <value>
        ///     From time UTC.
        /// </value>
        public TimeSpan FromTimeUtc { get; }

        /// <summary>
        ///     Gets to time UTC.
        /// </summary>
        /// <value>
        ///     To time UTC.
        /// </value>
        public TimeSpan ToTimeUtc { get; }

        /// <summary>
        ///     Gets the user hash.
        /// </summary>
        /// <value>
        ///     The user hash.
        /// </value>
        public UserHash UserHash { get; }

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

            var now = DateTime.UtcNow.TimeOfDay;
            return now >= FromTimeUtc && now <= ToTimeUtc;
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