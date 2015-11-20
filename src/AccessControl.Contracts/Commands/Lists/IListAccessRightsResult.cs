using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListAccessRights" /> command.
    /// </summary>
    [ContractClass(typeof(IListAccessRightsResultContract))]
    public interface IListAccessRightsResult
    {
        /// <summary>
        /// Gets the user-specific access rights.
        /// </summary>
        /// <value>
        /// The user-specific access rights.
        /// </value>
        IUserAccessRights[] UserAccessRights { get; }

        /// <summary>
        /// Gets the user group-specific access rights.
        /// </summary>
        /// <value>
        /// The user group-specific access rights.
        /// </value>
        IUserGroupAccessRights[] UserGroupAccessRights { get; }
    }
}