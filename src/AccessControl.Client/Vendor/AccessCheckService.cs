using System.Diagnostics.Contracts;
using AccessControl.Client.Data;
using Vendor.API;

namespace AccessControl.Client.Vendor
{
    internal class AccessCheckService : IAccessCheckService
    {
        private readonly IAccessPermissionCollection _accessPermissions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessCheckService" /> class.
        /// </summary>
        /// <param name="accessPermissions">The access permissions.</param>
        public AccessCheckService(IAccessPermissionCollection accessPermissions)
        {
            Contract.Requires(accessPermissions != null);
            _accessPermissions = accessPermissions;
        }

        /// <summary>
        ///     Tries the pass.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public bool TryPass(AccessCheckDto dto)
        {
            return _accessPermissions.IsAllowed(dto.AccessPointId, dto.UserHash);
        }
    }
}