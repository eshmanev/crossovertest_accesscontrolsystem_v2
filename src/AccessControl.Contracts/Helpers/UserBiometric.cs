using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class UserBiometric : User, IUserBiometric
    {
        public UserBiometric(string site, string userName)
            : base(site, userName)
        {
        }

        public string BiometricHash { get; set; }
    }
}