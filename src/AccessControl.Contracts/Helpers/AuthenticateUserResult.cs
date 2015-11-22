using AccessControl.Contracts.Commands.Security;

namespace AccessControl.Contracts.Helpers
{
    public class AuthenticateUserResult : IAuthenticateUserResult
    {
        public AuthenticateUserResult(bool authenticated, string ticket)
        {
            Authenticated = authenticated;
            Ticket = ticket;
        }

        public bool Authenticated { get; }
        public string Ticket { get; }

        public static IAuthenticateUserResult Failed()
        {
            return new AuthenticateUserResult(false, null);
        }

        public static IAuthenticateUserResult Success(string ticket)
        {
            return new AuthenticateUserResult(true, ticket);
        }
    }
}