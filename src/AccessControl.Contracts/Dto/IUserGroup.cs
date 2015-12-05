using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    /// <summary>
    ///     Represents a user group.
    /// </summary>
    [ContractClass(typeof(IUserGroupContract))]
    public interface IUserGroup
    {
        /// <summary>
        ///     Gets the group's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        string DisplayName { get; }
    }
}