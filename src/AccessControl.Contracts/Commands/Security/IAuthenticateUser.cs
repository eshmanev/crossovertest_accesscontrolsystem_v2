using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    ///     Authenticates the user.
    /// </summary>
    [ContractClass(typeof(IAuthenticateUserContract))]
    public interface IAuthenticateUser
    {
        /// <summary>
        ///     Gets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        string Password { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}