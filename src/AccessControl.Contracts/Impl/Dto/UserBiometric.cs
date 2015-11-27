using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class UserBiometric : User, IUserBiometric
    {
        public UserBiometric(string site, string userName, string[] userGroups)
            : base(site, userName, userGroups)
        {
        }

        public string BiometricHash { get; set; }
    }
}