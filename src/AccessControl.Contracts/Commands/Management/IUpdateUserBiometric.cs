using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    ///     Updates user's biometric info.
    /// </summary>
    [ContractClass(typeof(IUpdateUserBiometricContract))]
    public interface IUpdateUserBiometric
    {
        /// <summary>
        ///     Gets the biometric hash.
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