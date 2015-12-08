using System.Diagnostics;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class ScheduledGroupAccessStrategy : UserGroupAccessStrategyBase<ScheduledAccessRule>
    {
        private readonly IWeeklySchedule _schedule;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScheduledGroupAccessStrategy" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findGroupRequest">The find group request.</param>
        /// <param name="listUsersInGroupRequest">The list users in group request.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="schedule">The schedule.</param>
        public ScheduledGroupAccessStrategy(IDatabaseContext databaseContext,
                                            IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> findGroupRequest,
                                            IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest,
                                            string groupName,
                                            IWeeklySchedule schedule = null)
            : base(databaseContext, findGroupRequest, listUsersInGroupRequest, groupName)
        {
            _schedule = schedule;
        }

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
        public override bool UpdateAccessRule(AccessRuleBase rule)
        {
            Debug.Assert(_schedule != null);

            var scheduledRule = (ScheduledAccessRule) rule;
            scheduledRule.Update(_schedule);
            return true;
        }

        /// <summary>
        ///     Publishes an access granted event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override async Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            Debug.Assert(_schedule != null);

            var tuple = await ListUsersInGroup();
            await bus.Publish(new ScheduledGroupAccessAllowed(accessPoint.AccessPointId, GroupName, tuple.Item1, tuple.Item2, _schedule));
        }

        /// <summary>
        ///     Publishes an access denied event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            return bus.Publish(new ScheduledGroupAccessDenied(accessPoint.AccessPointId, GroupName));
        }
    }
}