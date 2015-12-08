using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal abstract class UserPermissionBase : IAccessPermission
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledUserAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        protected UserPermissionBase(Guid accessPointId, UserHash userHash)
        {
            Contract.Requires(accessPointId != Guid.Empty);

            AccessPointId = accessPointId;
            UserHash = userHash;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

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
        public abstract bool IsAllowed(Guid accessPointId, string userHash);

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public abstract void Accept(IAccessPermissionVisitor visitor);

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
            return GetType().GetHashCode() ^ AccessPointId.GetHashCode() ^ UserHash.UserName.GetHashCode();
        }
    }
}