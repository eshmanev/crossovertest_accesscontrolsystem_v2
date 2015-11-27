using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Commands
{
    public class UnregisterAccessPoint : IUnregisterAccessPoint
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnregisterAccessPoint" /> class.
        /// </summary>
        /// <param name="accessPoint">The access point.</param>
        public UnregisterAccessPoint(IAccessPoint accessPoint)
        {
            Contract.Requires(accessPoint != null);
            AccessPointId = accessPoint.AccessPointId;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnregisterAccessPoint" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        public UnregisterAccessPoint(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            AccessPointId = accessPointId;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }
    }
}