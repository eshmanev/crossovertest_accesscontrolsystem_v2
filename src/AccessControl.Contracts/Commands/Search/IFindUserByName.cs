using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Searches for user by name.
    /// </summary>
    [ContractClass(typeof(IFindUserByNameContract))]
    public interface IFindUserByName
    {
        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}