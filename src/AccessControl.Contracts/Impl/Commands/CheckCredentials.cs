using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class CheckCredentials : ICheckCredentials
    {
        public CheckCredentials(string userName, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            UserName = userName;
            Password = password;
        }

        public string Password { get; }
        public string UserName { get; }
    }

    public class CheckCredentialsResult : ICheckCredentialsResult
    {
        public CheckCredentialsResult(IUser user)
        {
            Valid = user != null;
            User = user;
        }

        public IUser User { get; }
        public bool Valid { get; }
    }
}