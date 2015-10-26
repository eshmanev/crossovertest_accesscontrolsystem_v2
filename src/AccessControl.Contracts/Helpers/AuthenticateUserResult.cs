using AccessControl.Contracts.Commands;

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