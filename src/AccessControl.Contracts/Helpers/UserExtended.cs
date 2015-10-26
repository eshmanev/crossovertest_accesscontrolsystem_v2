using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class UserExtended : User, IUserExtended
    {
        public UserExtended(string site, string userName)
            : base(site, userName)
        {
        }

        public string BiometricHash { get; set; }
    }
}