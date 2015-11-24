using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class User : IUser
    {
        public User(string site, string userName, string[] groups)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(groups != null);
            UserName = userName;
            Site = site;
            Groups = groups;
        }

        public string UserName { get; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string[] Groups { get; }
        public bool IsManager { get; set; }
        public string PhoneNumber { get; set; }
        public string Site { get; }
        public string Department { get; set; }
    }
}