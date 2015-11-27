using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class UserBiometricUpdated : IUserBiometricUpdated
    {
        public UserBiometricUpdated(string userName, string oldHash, string newHash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
            OldBiometricHash = oldHash;
            NewBiometricHash = newHash;
        }

        public string UserName { get; }
        public string OldBiometricHash { get; }
        public string NewBiometricHash { get; }
    }
}