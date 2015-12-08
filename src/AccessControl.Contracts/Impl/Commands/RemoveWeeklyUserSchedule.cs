using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.Impl.Commands
{
    public class RemoveUserSchedule : IRemoveUserSchedule
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoveUserSchedule" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public RemoveUserSchedule(Guid accessPointId, string userName)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            AccessPointId = accessPointId;
            UserName = userName;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user name.
        /// </summary>
        public string UserName { get; }
    }
}