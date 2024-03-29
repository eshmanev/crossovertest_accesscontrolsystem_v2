﻿using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when user's biometric information is updated.
    /// </summary>
    [ContractClass(typeof(IUserBiometricUpdatedContract))]
    public interface IUserBiometricUpdated
    {
        /// <summary>
        ///     Gets the biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        string NewBiometricHash { get; }

        /// <summary>
        ///     Gets the biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        string OldBiometricHash { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}