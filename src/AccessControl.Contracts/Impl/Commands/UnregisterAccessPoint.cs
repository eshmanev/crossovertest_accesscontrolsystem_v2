using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class UnregisterAccessPoint : IUnregisterAccessPoint
    {
        public UnregisterAccessPoint(IAccessPoint accessPoint)
        {
            Contract.Requires(accessPoint != null);
            AccessPointId = accessPoint.AccessPointId;
        }

        public UnregisterAccessPoint(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            AccessPointId = accessPointId;
        }

        public Guid AccessPointId { get; }
    }
}