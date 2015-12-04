using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class User : IUser
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="groups">The groups.</param>
        public User(string userName, string[] groups)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(groups != null);
            UserName = userName;
            Groups = groups;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }

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
        ///     Gets the user groups.
        /// </summary>
        /// <value>
        ///     The groups.
        /// </value>
        public string[] Groups { get; }

        /// <summary>
        ///     Gets a value indicating whether this user is a manager.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this user is a manager; otherwise, <c>false</c>.
        /// </value>
        public bool IsManager { get; set; }

        /// <summary>
        ///     Gets the name of the manager.
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
        ///     Gets the department.
        /// </summary>
        /// <value>
        ///     The department.
        /// </value>
        public string Department { get; set; }
    }
}