using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;

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
        /// <param name="user">The user, if any.</param>
        public AuthenticateUserResult(bool authenticated, string ticket, IUser user)
        {
            Authenticated = authenticated;
            Ticket = ticket;
            User = user;
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
        ///     Gets the authenticated user, if any.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public IUser User { get; }

        /// <summary>
        ///     Creates failed authentication result.
        /// </summary>
        /// <returns></returns>
        public static IAuthenticateUserResult Failed()
        {
            return new AuthenticateUserResult(false, null, null);
        }

        /// <summary>
        ///     Creates successfull authentication result with the specified AUTH ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="user">The authenticated user.</param>
        /// <returns></returns>
        public static IAuthenticateUserResult Success(string ticket, IUser user)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(ticket));
            Contract.Requires(user != null);
            return new AuthenticateUserResult(true, ticket, user);
        }
    }
}