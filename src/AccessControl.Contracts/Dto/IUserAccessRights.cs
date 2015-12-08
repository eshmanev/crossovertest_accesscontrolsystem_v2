using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    /// <summary>
    ///     Represents a user-specific access rights.
    /// </summary>
    [ContractClass(typeof(IUserAccessRightsContract))]
    public interface IUserAccessRights
    {
        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        ///     Gets the permanent access rules.
        /// </summary>
        /// <value>
        ///     The permanent access rules.
        /// </value>
        IPermanentAccessRule[] PermanentAccessRules { get; }

        /// <summary>
        ///     Gets the scheduled access rules.
        /// </summary>
        /// <value>
        ///     The scheduled access rules.
        /// </value>
        IScheduledAccessRule[] ScheduledAccessRules { get; }
    }
}