using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Finds user by biometric hash.
    /// </summary>
    [ContractClass(typeof(IFindUserByBiometricsContract))]
    public interface IFindUserByBiometrics
    {
        /// <summary>
        ///     Gets the biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        string BiometricHash { get; }
    }
}