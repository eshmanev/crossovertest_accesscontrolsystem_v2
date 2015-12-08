using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class PermanentUserAccess : UserPermissionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentUserAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        public PermanentUserAccess(Guid accessPointId, UserHash userHash)
            :base(accessPointId, userHash)
        {
        }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessPermissionVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        public override bool IsAllowed(Guid accessPointId, string userHash)
        {
            return AccessPointId == accessPointId && string.Equals(UserHash.Hash, userHash);
        }
    }
}