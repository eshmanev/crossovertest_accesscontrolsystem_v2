using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Allows access to an access point for a specific user.
    /// </summary>
    [ContractClass(typeof(IAllowUserAccessContract))]
    public interface IAllowUserAccess
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user name.
        /// </summary>
        string UserName { get; }
    }
}