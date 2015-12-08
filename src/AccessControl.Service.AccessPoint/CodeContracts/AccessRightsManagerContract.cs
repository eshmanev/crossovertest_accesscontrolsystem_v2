using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts.Dto;
using AccessControl.Service.AccessPoint.Services;

namespace AccessControl.Service.AccessPoint.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessRightsManager" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessRightsManager))]
    // ReSharper disable once InconsistentNaming
    internal abstract class AccessRightsManagerContract : IAccessRightsManager
    {
        public Task<IVoidResult> AllowAccess(Guid accessPointId, IAccessManagementStrategy strategy)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(strategy != null);
            Contract.Ensures(Contract.Result<Task<IVoidResult>>() != null);
            return null;
        }

        public Task<IVoidResult> DenyAccess(Guid accessPointId, IAccessManagementStrategy strategy)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(strategy != null);
            Contract.Ensures(Contract.Result<Task<IVoidResult>>() != null);
            return null;
        }
    }
}