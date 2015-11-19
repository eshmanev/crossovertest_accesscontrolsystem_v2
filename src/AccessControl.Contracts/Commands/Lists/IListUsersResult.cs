using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListUsers" /> command.
    /// </summary>
    [ContractClass(typeof(IListUsersResultContract))]
    public interface IListUsersResult
    {
        /// <summary>
        ///     Gets the users.
        /// </summary>
        /// <value>
        ///     The users.
        /// </value>
        IUser[] Users { get; }
    }
}