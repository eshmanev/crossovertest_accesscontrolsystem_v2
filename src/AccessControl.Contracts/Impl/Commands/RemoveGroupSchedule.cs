using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.Impl.Commands
{
    public class RemoveGroupSchedule : IRemoveGroupSchedule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoveGroupSchedule" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userGroupName">Name of the user group.</param>
        public RemoveGroupSchedule(Guid accessPointId, string userGroupName)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userGroupName));

            AccessPointId = accessPointId;
            UserGroupName = userGroupName;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user group name.
        /// </summary>
        public string UserGroupName { get; }
    }
}