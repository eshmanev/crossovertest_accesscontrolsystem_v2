using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListUsersBiometric" /> command.
    /// </summary>
    [ContractClass(typeof(IListUsersBiometricResultContract))]
    public interface IListUsersBiometricResult
    {
        /// <summary>
        ///     Gets the users.
        /// </summary>
        /// <value>
        ///     The users.
        /// </value>
        IUserBiometric[] Users { get; }
    }
}