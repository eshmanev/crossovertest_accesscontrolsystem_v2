using System;
using System.Diagnostics.Contracts;
using AccessControl.Client.CodeContracts;

namespace AccessControl.Client.Data
{
    /// <summary>
    ///     Defines an access permission.
    /// </summary>
    [ContractClass(typeof(IAccessPermissionContract))]
    internal interface IAccessPermission
    {
        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        bool IsAllowed(Guid accessPointId, string userHash);

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(IAccessPermissionVisitor visitor);
    }
}