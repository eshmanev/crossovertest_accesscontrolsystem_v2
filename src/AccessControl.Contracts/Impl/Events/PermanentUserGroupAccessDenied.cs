using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class PermanentUserGroupAccessDenied : IPermanentUserGroupAccessDenied
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentUserGroupAccessDenied" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        public PermanentUserGroupAccessDenied(Guid accessPointId, string userGroupName)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
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
    }
}