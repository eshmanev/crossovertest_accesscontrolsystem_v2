using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Data.Entities;
using AccessControl.Service.AccessPoint.Services;
using MassTransit;

namespace AccessControl.Service.AccessPoint.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessManagementStrategy" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessManagementStrategy))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAllowAccessStrategyContract : IAccessManagementStrategy
    {
        public Task<string> ValidateRequest()
        {
            Contract.Ensures(Contract.Result<Task>() != null);
            return null;
        }

        public AccessRightsBase FindAccessRights()
        {
            return null;
        }

        public AccessRightsBase CreateAccessRights()
        {
            Contract.Ensures(Contract.Result<AccessRightsBase>() != null);
            return null;
        }

        public AccessRuleBase FindAccessRule(AccessRightsBase accessRights, Data.Entities.AccessPoint accessPoint)
        {
            Contract.Requires(accessRights != null);
            Contract.Requires(accessPoint != null);
            return null;
        }

        public AccessRuleBase CreateAccessRule()
        {
            Contract.Ensures(Contract.Result<AccessRuleBase>() != null);
            return null;
        }

        public Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            Contract.Requires(bus != null);
            Contract.Requires(accessPoint != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            return null;
        }

        public Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            Contract.Requires(bus != null);
            Contract.Requires(accessPoint != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            return null;
        }
    }
}