using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when permanent access is denied for a user group.
    /// </summary>
    [ContractClass(typeof(IPermanentGroupAccessDeniedContract))]
    public interface IPermanentGroupAccessDenied
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the name of the user group.
        /// </summary>
        /// <value>
        ///     The name of the user group.
        /// </value>
        string UserGroupName { get; }
    }
}