using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class AuthenticateUser : IAuthenticateUser
    {
        public AuthenticateUser(string userName, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            UserName = userName;
            Password = password;
        }
        public string UserName { get; }
        public string Password { get; }
    }
}