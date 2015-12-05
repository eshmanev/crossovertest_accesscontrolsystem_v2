using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    /// <summary>
    ///     Defines an authentication ticket.
    /// </summary>
    [ContractClass(typeof(ITicketContract))]
    public interface ITicket
    {
        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the roles.
        /// </summary>
        /// <value>
        ///     The roles.
        /// </value>
        string[] Roles { get; }

        /// <summary>
        ///     Gets the names of managers who delegated their management rights to the user.
        /// </summary>
        /// <value>
        ///     An array of the user names.
        /// </value>
        string[] OnBehalfOf { get; }
    }
}