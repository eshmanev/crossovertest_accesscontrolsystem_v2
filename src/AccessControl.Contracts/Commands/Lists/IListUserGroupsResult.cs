using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListUserGroups" /> command.
    /// </summary>
    [ContractClass(typeof(IListUserGroupsResultContract))]
    public interface IListUserGroupsResult
    {
        /// <summary>
        ///     Gets an array of user groups.
        /// </summary>
        IUserGroup[] Groups { get; }
    }
}