using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when a permanent access is allowed for a user.
    /// </summary>
    [ContractClass(typeof(IPermanentUserAccessAllowedContract))]
    public interface IPermanentUserAccessAllowed
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user's biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        string BiometricHash { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}