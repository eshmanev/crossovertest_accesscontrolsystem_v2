using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class PermanentUserAccessStrategy : UserAccessStrategyBase<PermanentAccessRule>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentUserAccessStrategy" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findUserRequest">The find user request.</param>
        /// <param name="userName">Name of the user.</param>
        public PermanentUserAccessStrategy(IDatabaseContext databaseContext, IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest, string userName)
            : base(databaseContext, findUserRequest, userName)
        {
        }

        /// <summary>
        ///     Updates the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>
        ///     true if the rule was updated; otherwise, false.
        /// </returns>
        public override bool UpdateAccessRule(AccessRuleBase rule)
        {
            return false;
        }

        /// <summary>
        ///     Publishes the <see cref="PermanentUserAccessAllowed" /> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            var hash = FindUserHash(UserName);
            return bus.Publish(new PermanentUserAccessAllowed(accessPoint.AccessPointId, UserName, hash));
        }


        /// <summary>
        ///     Publishes the <see cref="PermanentUserAccessDenied" /> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            return bus.Publish(new PermanentUserAccessDenied(accessPoint.AccessPointId, UserName));
        }
    }
}