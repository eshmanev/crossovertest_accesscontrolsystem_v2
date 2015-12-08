using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Client.Data
{
    internal abstract class GroupPermisionBase : IAccessPermission
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentGroupAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <param name="userHashes">The user hashes.</param>
        protected GroupPermisionBase(Guid accessPointId, string userGroupName, UserHash[] userHashes)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));
            Contract.Requires(userHashes != null);

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
            UserHashes = userHashes;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the name of the user group.
        /// </summary>
        /// <value>
        ///     The name of the user group.
        /// </value>
        public string UserGroupName { get; }

        /// <summary>
        ///     Gets the user hashes.
        /// </summary>
        /// <value>
        ///     The user hashes.
        /// </value>
        public UserHash[] UserHashes { get; }

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
            var other = obj as PermanentGroupAccess;
            return other != null && other.AccessPointId == AccessPointId && string.Equals(other.UserGroupName, UserGroupName);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ AccessPointId.GetHashCode() ^ UserGroupName.GetHashCode();
        }
    }
}