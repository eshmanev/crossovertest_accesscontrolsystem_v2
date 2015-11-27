using AccessControl.Contracts.Commands.Security;

namespace AccessControl.Contracts.Impl.Commands
{
    /// <summary>
    /// </summary>
    public class AuthenticateUserResult : IAuthenticateUserResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticateUserResult" /> class.
        /// </summary>
        /// <param name="authenticated">if set to <c>true</c> [authenticated].</param>
        /// <param name="ticket">The ticket.</param>
        public AuthenticateUserResult(bool authenticated, string ticket)
        {
            Authenticated = authenticated;
            Ticket = ticket;
        }

        /// <summary>
        ///     Gets a value indicating whether the user is authenticated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool Authenticated { get; }

        /// <summary>
        ///     Gets the ticket used to access services.
        /// </summary>
        /// <value>
        ///     The ticket.
        /// </value>
        public string Ticket { get; }

        /// <summary>
        ///     Creates failed authentication result.
        /// </summary>
        /// <returns></returns>
        public static IAuthenticateUserResult Failed()
        {
            return new AuthenticateUserResult(false, null);
        }

        /// <summary>
        ///     Creates successfull authentication result with the specified AUTH ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public static IAuthenticateUserResult Success(string ticket)
        {
            return new AuthenticateUserResult(true, ticket);
        }
    }
}