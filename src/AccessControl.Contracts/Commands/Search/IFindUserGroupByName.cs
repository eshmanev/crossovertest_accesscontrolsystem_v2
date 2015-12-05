using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Searches for a user group.
    /// </summary>
    [ContractClass(typeof(IFindUserGroupByNameContract))]
    public interface IFindUserGroupByName
    {
        /// <summary>
        ///     Gets the user group.
        /// </summary>
        /// <value>
        ///     The user group.
        /// </value>
        string UserGroup { get; }
    }
}