using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class User : IUser
    {
        public User(string site, string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
            Site = site;
        }

        public string UserName { get; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Site { get; }
        public string Department { get; set; }
    }
}