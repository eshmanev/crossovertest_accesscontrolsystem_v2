using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    /// <summary>
    ///     Represents a user group-specific access rights.
    /// </summary>
    [ContractClass(typeof(IUserGroupAccessRightsContract))]
    public interface IUserGroupAccessRights
    {
        /// <summary>
        ///     Gets the name of the user group.
        /// </summary>
        /// <value>
        ///     The name of the user group.
        /// </value>
        string UserGroupName { get; }

        /// <summary>
        ///     Gets the permanent access rules.
        /// </summary>
        /// <value>
        ///     The permanent access rules.
        /// </value>
        IPermanentAccessRule[] PermanentAccessRules { get; }
    }
}