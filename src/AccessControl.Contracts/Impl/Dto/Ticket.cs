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
        /// <param name="domain">The domain.</param>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="onBehalfOf">The names of managers who delegated their management rights to the user.</param>
        public Ticket(string domain, IUser user, string[] roles, string[] onBehalfOf)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(domain));
            Contract.Requires(user != null);
            Contract.Requires(roles != null);
            Contract.Requires(onBehalfOf != null);

            Domain = domain;
            User = new UserInfo(user);
            Roles = roles;
            OnBehalfOf = onBehalfOf;
        }

        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public UserInfo User { get; set; }

        /// <summary>
        ///     Gets the domain for the authenticated user.
        /// </summary>
        /// <value>
        ///     The domain.
        /// </value>
        public string Domain { get; set; }

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

        IUser ITicket.User => User;

        /// <summary>
        ///     Serializable user info.
        /// </summary>
        public struct UserInfo : IUser
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="UserInfo" /> struct.
            /// </summary>
            /// <param name="user">The user.</param>
            public UserInfo(IUser user)
            {
                Contract.Requires(user != null);

                Department = user.Department;
                DisplayName = user.DisplayName;
                Email = user.Email;
                PhoneNumber = user.PhoneNumber;
                Site = user.Site;
                UserName = user.UserName;
                Groups = user.Groups;
                IsManager = user.IsManager;
                ManagerName = user.ManagerName;
            }

            /// <summary>
            ///     Gets the department.
            /// </summary>
            /// <value>
            ///     The department.
            /// </value>
            public string Department { get; set; }

            /// <summary>
            ///     Gets the display name.
            /// </summary>
            /// <value>
            ///     The display name.
            /// </value>
            public string DisplayName { get; set; }

            /// <summary>
            ///     Gets the email.
            /// </summary>
            /// <value>
            ///     The email.
            /// </value>
            public string Email { get; set; }

            /// <summary>
            ///     Gets the groups.
            /// </summary>
            /// <value>
            ///     The groups.
            /// </value>
            public string[] Groups { get; set; }

            /// <summary>
            ///     Gets a value indicating whether this instance is manager.
            /// </summary>
            /// <value>
            ///     <c>true</c> if this instance is manager; otherwise, <c>false</c>.
            /// </value>
            public bool IsManager { get; set; }

            /// <summary>
            ///     Gets or sets the name of the manager.
            /// </summary>
            /// <value>
            ///     The name of the manager.
            /// </value>
            public string ManagerName { get; set; }

            /// <summary>
            ///     Gets the phone number.
            /// </summary>
            /// <value>
            ///     The phone number.
            /// </value>
            public string PhoneNumber { get; set; }

            /// <summary>
            ///     Gets the site.
            /// </summary>
            /// <value>
            ///     The site.
            /// </value>
            public string Site { get; set; }

            /// <summary>
            ///     Gets the name of the user.
            /// </summary>
            /// <value>
            ///     The name of the user.
            /// </value>
            public string UserName { get; set; }
        }
    }
}