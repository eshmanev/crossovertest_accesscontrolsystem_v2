using System;
using System.Diagnostics.Contracts;
using AccessControl.Client.Data;

namespace AccessControl.Client.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessPermission" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessPermission))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAccessPermissionContract : IAccessPermission
    {
        public bool IsAllowed(Guid accessPointId, string userHash)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(userHash));
            return false;
        }

        public void Accept(IAccessPermissionVisitor visitor)
        {
            Contract.Requires(visitor != null);
        }
    }
}