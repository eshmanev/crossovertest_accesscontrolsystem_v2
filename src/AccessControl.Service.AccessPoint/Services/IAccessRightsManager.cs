using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts.Dto;
using AccessControl.Service.AccessPoint.CodeContracts;

namespace AccessControl.Service.AccessPoint.Services
{
    /// <summary>
    ///     Manages access rights.
    /// </summary>
    [ContractClass(typeof(AccessRightsManagerContract))]
    internal interface IAccessRightsManager
    {
        /// <summary>
        ///     Allows access for the specified access point.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns></returns>
        Task<IVoidResult> AllowAccess(Guid accessPointId, IAccessManagementStrategy strategy);

        /// <summary>
        ///     Denies access for the specified access point..
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns></returns>
        Task<IVoidResult> DenyAccess(Guid accessPointId, IAccessManagementStrategy strategy);
    }
}