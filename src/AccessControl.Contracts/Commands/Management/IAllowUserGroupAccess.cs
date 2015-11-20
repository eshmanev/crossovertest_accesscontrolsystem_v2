using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Allows access to an access point for a specific user group.
    /// </summary>
    [ContractClass(typeof(IAllowUserGroupAccessContract))]
    public interface IAllowUserGroupAccess
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