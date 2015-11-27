using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.Impl.Commands
{
    public class UpdateUserBiometric : IUpdateUserBiometric
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateUserBiometric" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="biometricHash">The biometric hash.</param>
        public UpdateUserBiometric(string userName, string biometricHash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
            BiometricHash = biometricHash;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }

        /// <summary>
        ///     Gets the biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        public string BiometricHash { get; }
    }
}