using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Helpers;
using MassTransit;
using Vendor.API;

namespace AccessControl.Client.Vendor
{
    public class AccessPointRegistry : IAccessPointRegistry
    {
        private readonly IBus _bus;

        public AccessPointRegistry(IBus bus)
        {
            Contract.Requires(bus != null);
            _bus = bus;
        }

        public void RegisterAccessPoint(AccessPointDto dto)
        {
            var message = new RegisterAccessPoint(dto.AccessPointId, dto.Name, dto.Description);
            _bus.Publish(message);
        }

        public void UnregisterAccessPoint(Guid accessPointId)
        {
            throw new NotImplementedException();
        }
    }
}