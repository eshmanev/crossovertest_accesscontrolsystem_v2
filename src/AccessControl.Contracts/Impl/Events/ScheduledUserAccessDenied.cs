using System;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class ScheduledUserAccessDenied : IScheduledUserAccessDenied
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledUserAccessDenied" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public ScheduledUserAccessDenied(Guid accessPointId, string userName)
        {
            AccessPointId = accessPointId;
            UserName = userName;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }
    }
}