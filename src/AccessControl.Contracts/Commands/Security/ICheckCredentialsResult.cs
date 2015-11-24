using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Security
{
    /// <summary>
    ///     Represents a result of the <see cref="ICheckCredentials" /> request.
    /// </summary>
    [ContractClass(typeof(ICheckCredentialsResultContract))]
    public interface ICheckCredentialsResult
    {
        /// <summary>
        ///     If credentials are valid this property contains the authenticated user; otherwise, null.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        IUser User { get; }

        /// <summary>
        ///     Gets a value indicating whether the credentials are valid.
        /// </summary>
        bool Valid { get; }
    }
}