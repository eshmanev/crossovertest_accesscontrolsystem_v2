using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Client.CodeContracts;

namespace AccessControl.Client.Data
{
    [ContractClass(typeof(IAccessPermissionCollectionContract))]
    internal interface IAccessPermissionCollection : IEnumerable<IAccessPermission>
    {
        /// <summary>
        ///     Adds or updates the given permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        void AddOrUpdatePermission(IAccessPermission permission);

        /// <summary>
        ///     Removes the given permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        void RemovePermission(IAccessPermission permission);

        /// <summary>
        ///     Determines whether the specified user hash is allowed.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userHash">The user hash.</param>
        /// <returns></returns>
        bool IsAllowed(Guid accessPointId, string userHash);
    }
}