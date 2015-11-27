using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListDelegatedUsers" /> command.
    /// </summary>
    [ContractClass(typeof(IListDelegatedUsersResultContract))]
    public interface IListDelegatedUsersResult
    {
        /// <summary>
        ///     Gets the users' names who are delegated management and monitoring rights.
        /// </summary>
        /// <value>
        ///     The users.
        /// </value>
        string[] UserNames { get; }
    }
}