using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Data;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class AccessRightsManager : IAccessRightsManager
    {
        private readonly IBus _bus;
        private readonly IDatabaseContext _databaseContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessRightsManager" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="databaseContext">The database context.</param>
        public AccessRightsManager(IBus bus, IDatabaseContext databaseContext)
        {
            Contract.Requires(bus != null);
            Contract.Requires(databaseContext != null);
            _bus = bus;
            _databaseContext = databaseContext;
        }

        /// <summary>
        ///     Allows access for the specified access point.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns></returns>
        public async Task<IVoidResult> AllowAccess(Guid accessPointId, IAccessManagementStrategy strategy)
        {
            var errorMessage = await strategy.ValidateRequest();
            if (errorMessage != null)
            {
                return new VoidResult(errorMessage);
            }

            var accessPoint = _databaseContext.AccessPoints.GetById(accessPointId);
            if (accessPoint == null)
            {
                return new VoidResult($"Access point {accessPointId} is not registered.");
            }

            var accessRights = strategy.FindAccessRights() ?? strategy.CreateAccessRights();
            if (strategy.FindAccessRule(accessRights, accessPoint) != null)
            {
                return new VoidResult();
            }


            var rule = strategy.CreateAccessRule();
            rule.AccessPoint = accessPoint;
            accessRights.AddAccessRule(rule);
            if (accessRights.Id == 0)
            {
                _databaseContext.AccessRights.Insert(accessRights);
            }
            else
            {
                _databaseContext.AccessRights.Update(accessRights);
            }
            _databaseContext.Commit();

            await strategy.OnAccessGranted(_bus, accessPoint);
            return new VoidResult();
        }

        /// <summary>
        ///     Denies access for the specified access point..
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="strategy">The strategy.</param>
        /// <returns></returns>
        public async Task<IVoidResult> DenyAccess(Guid accessPointId, IAccessManagementStrategy strategy)
        {
            var errorMessage = await strategy.ValidateRequest();
            if (errorMessage != null)
            {
                return new VoidResult(errorMessage);
            }

            var accessRights = strategy.FindAccessRights();
            if (accessRights == null)
            {
                return new VoidResult();
            }

            var accessPoint = _databaseContext.AccessPoints.GetById(accessPointId);
            if (accessPoint == null)
            {
                return new VoidResult($"Access point {accessPointId} is not registered.");
            }

            var accessRule = strategy.FindAccessRule(accessRights, accessPoint);
            if (accessRule == null)
            {
                return new VoidResult();
            }

            accessRights.RemoveAccessRule(accessRule);
            if (accessRights.AccessRules.Any())
            {
                _databaseContext.AccessRights.Update(accessRights);
            }

            else
            {
                _databaseContext.AccessRights.Delete(accessRights);
            }
            _databaseContext.Commit();

            await strategy.OnAccessDenied(_bus, accessPoint);
            return new VoidResult();
        }
    }
}