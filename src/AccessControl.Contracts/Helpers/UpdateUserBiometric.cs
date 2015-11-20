using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.Helpers
{
    public class UpdateUserBiometric : IUpdateUserBiometric
    {
        public UpdateUserBiometric(string userName, string biometricHash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
            BiometricHash = biometricHash;
        }

        public string UserName { get; }
        public string BiometricHash { get; }
    }
}