using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Represents a result of the <see cref="IFindUserGroupByName" /> command.
    /// </summary>
    [ContractClass(typeof(IFindUserGroupByNameResultContract))]
    public interface IFindUserGroupByNameResult
    {
        /// <summary>
        ///     Gets the user group.
        /// </summary>
        /// <value>
        ///     The user group.
        /// </value>
        IUserGroup UserGroup { get; }
    }
}