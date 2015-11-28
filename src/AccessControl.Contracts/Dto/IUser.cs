using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    /// <summary>
    ///     Represents a user in LDAP directory.
    /// </summary>
    [ContractClass(typeof(IUserContract))]
    public interface IUser
    {
        /// <summary>
        ///     Gets the department.
        /// </summary>
        /// <value>
        ///     The department.
        /// </value>
        string Department { get; }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        string DisplayName { get; }

        /// <summary>
        ///     Gets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        string Email { get; }

        /// <summary>
        ///     Gets the user groups.
        /// </summary>
        /// <value>
        ///     The groups.
        /// </value>
        string[] Groups { get; }

        /// <summary>
        ///     Gets a value indicating whether this user is a manager.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this user is a manager; otherwise, <c>false</c>.
        /// </value>
        bool IsManager { get; }

        /// <summary>
        ///     Gets the name of the manager.
        /// </summary>
        /// <value>
        ///     The name of the manager.
        /// </value>
        string ManagerName { get; }

        /// <summary>
        ///     Gets the phone number.
        /// </summary>
        /// <value>
        ///     The phone number.
        /// </value>
        string PhoneNumber { get; }

        /// <summary>
        ///     Gets the site.
        /// </summary>
        /// <value>
        ///     The site.
        /// </value>
        string Site { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}