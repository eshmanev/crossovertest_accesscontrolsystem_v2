using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class Identity : IIdentity
    {
        private Identity()
        {
        }

        public Identity(string site, string department, string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            Department = department;
            Site = site;
            UserName = userName;
        }

        /// <summary>
        /// The empty identity.
        /// </summary>
        public static readonly IIdentity Empty = new Identity();

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(UserName);
        public string Department { get; }
        public string Site { get; }
        public string UserName { get; }
    }
}