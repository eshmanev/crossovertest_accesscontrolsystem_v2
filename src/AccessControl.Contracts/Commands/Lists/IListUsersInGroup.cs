using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Lists users in the specified user group.
    /// </summary>
    [ContractClass(typeof(IListUsersInGroupContract))]
    public interface IListUsersInGroup
    {
        /// <summary>
        ///     Gets the name of the user group.
        /// </summary>
        /// <value>
        ///     The name of the user group.
        /// </value>
        string UserGroupName { get; }
    }
}