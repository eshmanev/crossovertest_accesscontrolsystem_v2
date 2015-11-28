using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when a manager revokes his managment and monitoring rights.
    /// </summary>
    [ContractClass(typeof(IManagementRightsRevokedContract))]
    public interface IManagementRightsRevoked
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