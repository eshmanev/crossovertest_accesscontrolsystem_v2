using System;
using System.Linq;

namespace AccessControl.Client.Data
{
    [Serializable]
    internal class PermanentGroupAccess : GroupPermisionBase, IAccessPermission
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentGroupAccess" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        /// <param name="userHashes">The user hashes.</param>
        public PermanentGroupAccess(Guid accessPointId, string userGroupName, UserHash[] userHashes)
            : base(accessPointId, userGroupName, userHashes)
        {
        }

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        public override bool IsAllowed(Guid accessPointId, string userHash)
        {
            return AccessPointId == accessPointId && UserHashes.Any(x => string.Equals(userHash, (string) x.Hash));
        }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessPermissionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}