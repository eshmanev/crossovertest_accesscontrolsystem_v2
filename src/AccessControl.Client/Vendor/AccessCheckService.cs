using System.Diagnostics.Contracts;
using AccessControl.Client.Data;
using AccessControl.Contracts.Impl.Events;
using MassTransit;
using Vendor.API;

namespace AccessControl.Client.Vendor
{
    internal class AccessCheckService : IAccessCheckService
    {
        private readonly IBus _bus;
        private readonly IAccessPermissionCollection _accessPermissions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessCheckService" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPermissions">The access permissions.</param>
        public AccessCheckService(IBus bus, IAccessPermissionCollection accessPermissions)
        {
            Contract.Requires(bus != null);
            Contract.Requires(accessPermissions != null);
            _bus = bus;
            _accessPermissions = accessPermissions;
        }

        /// <summary>
        ///     Tries the pass.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public bool TryPass(AccessCheckDto dto)
        {
            var allowed = _accessPermissions.IsAllowed(dto.AccessPointId, dto.UserHash);
            _bus.Publish(new AccessAttempted(dto.AccessPointId, dto.UserHash, !allowed));
            return allowed;
        }
    }
}