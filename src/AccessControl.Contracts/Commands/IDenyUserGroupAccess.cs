using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    ///     Denies access to an access point for a specific user group.
    /// </summary>
    [ContractClass(typeof(IDenyUserGroupAccessContract))]
    public interface IDenyUserGroupAccess
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user group name.
        /// </summary>
        string UserGroupName { get; }
    }
}