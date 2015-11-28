using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindUserByBiometrics : IFindUserByBiometrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindUserByBiometrics" /> class.
        /// </summary>
        /// <param name="biometricHash">The biometric hash.</param>
        public FindUserByBiometrics(string biometricHash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(biometricHash));
            BiometricHash = biometricHash;
        }

        /// <summary>
        ///     Gets the biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        public string BiometricHash { get; }
    }
}