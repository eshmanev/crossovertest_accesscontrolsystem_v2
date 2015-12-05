using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Security
{
    /// <summary>
    ///     Checks the specified credentials.
    /// </summary>
    [ContractClass(typeof(ICheckCredentialsContract))]
    public interface ICheckCredentials
    {
        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        string Password { get; }
    }
}