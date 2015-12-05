using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    /// <summary>
    ///     Represents an authentication ticket.
    /// </summary>
    public class Ticket : ITicket
    {
        // Serialization.
        // ReSharper disable once UnusedMember.Local
        public Ticket()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Ticket" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="onBehalfOf">The names of managers who delegated their management rights to the user.</param>
        public Ticket(IUser user, string[] roles, string[] onBehalfOf)
        {
            Contract.Requires(user != null);
            Contract.Requires(roles != null);
            Contract.Requires(onBehalfOf != null);

            UserName = user.UserName;
            Roles = roles;
            OnBehalfOf = onBehalfOf;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets the roles.
        /// </summary>
        /// <value>
        ///     The roles.
        /// </value>
        public string[] Roles { get; set; }

        /// <summary>
        ///     Gets the names of managers who delegated their management rights to the user.
        /// </summary>
        /// <value>
        ///     An array of the user names.
        /// </value>
        public string[] OnBehalfOf { get; set; }
    }
}