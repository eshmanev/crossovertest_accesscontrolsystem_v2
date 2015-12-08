using System.Diagnostics;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class ScheduledUserAccessStrategy : UserAccessStrategyBase<ScheduledAccessRule>
    {
        private readonly IWeeklySchedule _schedule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledUserAccessStrategy" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findUserRequest">The find user request.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="schedule">The schedule.</param>
        public ScheduledUserAccessStrategy(IDatabaseContext databaseContext, IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest, string userName, IWeeklySchedule schedule = null)
            : base(databaseContext, findUserRequest, userName)
        {
            _schedule = schedule;
        }

        /// <summary>
        ///     Creates a new access rule.
        /// </summary>
        /// <returns></returns>
        public override AccessRuleBase CreateAccessRule()
        {
            Debug.Assert(_schedule != null);

            var rule = (ScheduledAccessRule) base.CreateAccessRule();
            rule.Update(_schedule);
            return rule;
        }

        /// <summary>
        ///     Updates the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns>
        ///     true if the rule was updated; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool UpdateAccessRule(AccessRuleBase rule)
        {
            Debug.Assert(_schedule != null);

            var scheduledRule = (ScheduledAccessRule) rule;
            scheduledRule.Update(_schedule);
            return true;
        }

        /// <summary>
        ///     Raises the <see cref="ScheduledUserAccessAllowed" /> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            Debug.Assert(_schedule != null);

            var hash = FindUserHash(UserName);
            return bus.Publish(new ScheduledUserAccessAllowed(accessPoint.AccessPointId, UserName, hash, _schedule));
        }

        /// <summary>
        ///     Raises the <see cref="ScheduledUserAccessDenied" /> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            return bus.Publish(new ScheduledUserAccessDenied(accessPoint.AccessPointId, UserName));
        }
    }
}