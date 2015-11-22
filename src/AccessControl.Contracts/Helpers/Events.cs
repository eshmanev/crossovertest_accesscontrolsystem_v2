using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Helpers
{
    public static class Events
    {
        public static IUserBiometricUpdated UserBiometricUpdated(string userName, string oldHash, string newHash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            return new UserBiometricUpdatedImpl
            {
                UserName = userName,
                OldBiometricHash = oldHash,
                NewBiometricHash = newHash
            };
        }

        private class UserBiometricUpdatedImpl : IUserBiometricUpdated
        {
            public string UserName { get; set; }
            public string OldBiometricHash { get; set; }
            public string NewBiometricHash { get; set; }
        }
    }
}