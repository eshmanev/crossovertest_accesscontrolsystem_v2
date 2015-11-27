using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Grants management rights to the specified user.
    /// </summary>
    [ContractClass(typeof(IGrantManagementRightsContract))]
    public interface IGrantManagementRights
    {
        /// <summary>
        ///     Gets the name of the user who are granted managment rights.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}