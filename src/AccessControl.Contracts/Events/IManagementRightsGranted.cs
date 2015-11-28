using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when a manager grants his managment and monitoring rights.
    /// </summary>
    [ContractClass(typeof(IManagementRightsGrantedContract))]
    public interface IManagementRightsGranted
    {
        /// <summary>
        ///     Gets the grantee.
        /// </summary>
        /// <value>
        ///     The grantee.
        /// </value>
        string Grantee { get; }

        /// <summary>
        ///     Gets the grantor.
        /// </summary>
        /// <value>
        ///     The grantor.
        /// </value>
        string Grantor { get; }
    }
}