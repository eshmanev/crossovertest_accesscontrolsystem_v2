using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class AuthenticateUserResult : IAuthenticateUserResult
    {
        public AuthenticateUserResult(string errorMessage)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(errorMessage));
            Authenticated = false;
            Roles = new string[0];
            Message = errorMessage;
        }

        public AuthenticateUserResult(IUser user, string[] roles)
        {
            Contract.Requires(user != null);
            Contract.Requires(roles != null);

            Authenticated = true;
            Message = null;
            User = user;
            Roles = roles;
        }

        public bool Authenticated { get; }
        public string Message { get; }
        public string[] Roles { get; }
        public IUser User { get; }
    }
}