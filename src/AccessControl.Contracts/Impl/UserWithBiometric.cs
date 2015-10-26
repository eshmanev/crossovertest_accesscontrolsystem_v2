namespace AccessControl.Contracts.Impl
{
    public class UserWithBiometric : User, IUserWithBiometric
    {
        public UserWithBiometric(string site, string userName)
            : base(site, userName)
        {
        }

        public string BiometricHash { get; set; }
    }
}