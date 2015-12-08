using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Data.Entities;
using AccessControl.Service.AccessPoint.CodeContracts;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    /// <summary>
    ///     Defines an access management strategy.
    /// </summary>
    [ContractClass(typeof(IAllowAccessStrategyContract))]
    internal interface IAccessManagementStrategy
    {
        /// <summary>
        ///     Validates the request and returns an error message.
        /// </summary>
        /// <returns></returns>
        Task<string> ValidateRequest();

        /// <summary>
        ///     Gets or creates the access rights.
        /// </summary>
        /// <returns></returns>
        AccessRightsBase FindAccessRights();

        /// <summary>
        ///     Creates a new access rights entity.
        /// </summary>
        /// <returns></returns>
        AccessRightsBase CreateAccessRights();

        /// <summary>
        ///     Searches a rule for the specified access point.
        /// </summary>
        /// <param name="accessRights">The access rights.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        AccessRuleBase FindAccessRule(AccessRightsBase accessRights, Data.Entities.AccessPoint accessPoint);

        /// <summary>
        ///     Creates a new access rule.
        /// </summary>
        /// <returns></returns>
        AccessRuleBase CreateAccessRule();

        /// <summary>
        /// Updates the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>true if the rule was updated; otherwise, false.</returns>
        bool UpdateAccessRule(AccessRuleBase rule);

        /// <summary>
        ///     Publishes an access granted event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint);

        /// <summary>
        ///     Publishes an access denied event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint);
    }
}