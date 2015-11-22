using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AccessControl.Client.Data;

namespace AccessControl.Client.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessPermissionCollection" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessPermissionCollection))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAccessPermissionCollectionContract : IAccessPermissionCollection
    {
        public IEnumerator<IAccessPermission> GetEnumerator()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddOrUpdatePermission(IAccessPermission permission)
        {
            Contract.Requires(permission != null);
        }

        public void RemovePermission(IAccessPermission permission)
        {
            Contract.Requires(permission != null);
        }

        public bool IsAllowed(Guid accessPointId, string userHash)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userHash));
            return false;
        }
    }
}