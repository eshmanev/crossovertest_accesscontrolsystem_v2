using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Services
{
    internal class PermanentGroupAccessStrategy : UserGroupAccessStrategyBase<PermanentAccessRule>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentGroupAccessStrategy" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="findGroupRequest">The find group request.</param>
        /// <param name="listUsersInGroupRequest">The list users in group request.</param>
        /// <param name="groupName">Name of the group.</param>
        public PermanentGroupAccessStrategy(IDatabaseContext databaseContext,
                                            IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> findGroupRequest,
                                            IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest,
                                            string groupName)
            : base(databaseContext, findGroupRequest, listUsersInGroupRequest, groupName)
        {
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
            return false;
        }

        /// <summary>
        ///     Publishes the <see cref="PermanentGroupAccessAllowed" /> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override async Task OnAccessGranted(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            var tuple = await ListUsersInGroup();
            await bus.Publish(new PermanentGroupAccessAllowed(accessPoint.AccessPointId, GroupName, tuple.Item1, tuple.Item2));
        }

        /// <summary>
        ///     Publishes the <see cref="PermanentGroupAccessDenied" /> event.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="accessPoint">The access point.</param>
        /// <returns></returns>
        public override Task OnAccessDenied(IBus bus, Data.Entities.AccessPoint accessPoint)
        {
            return bus.Publish(new PermanentGroupAccessDenied(accessPoint.AccessPointId, GroupName));
        }
    }
}