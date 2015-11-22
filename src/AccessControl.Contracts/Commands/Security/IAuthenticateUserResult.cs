using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Security
{
    /// <summary>
    ///     Represents a result of the <see cref="IAuthenticateUser" /> command.
    /// </summary>
    [ContractClass(typeof(IAuthenticateUserResultContract))]
    public interface IAuthenticateUserResult
    {
        /// <summary>
        ///     Gets a value indicating whether the user is authenticated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if authenticated; otherwise, <c>false</c>.
        /// </value>
        bool Authenticated { get; }

        /// <summary>
        ///     Gets the ticket used to access services.
        /// </summary>
        /// <value>
        ///     The ticket.
        /// </value>
        string Ticket { get; }
    }
}