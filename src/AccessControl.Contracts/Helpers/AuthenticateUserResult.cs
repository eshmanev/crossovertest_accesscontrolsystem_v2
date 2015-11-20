using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Security;

namespace AccessControl.Contracts.Helpers
{
    public class AuthenticateUserResult : IAuthenticateUserResult
    {
        public AuthenticateUserResult(bool result, string message = null)
        {
            Authenticated = result;
            Message = message;
        }

        public bool Authenticated { get; }
        public string Message { get; }
    }
}