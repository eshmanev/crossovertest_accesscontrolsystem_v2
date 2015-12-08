using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Data;
using AccessControl.Service.AccessPoint.Helpers;
using AccessControl.Service.AccessPoint.Services;
using AccessControl.Service.Security;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

namespace AccessControl.Service.AccessPoint.Consumers
{
    /// <summary>
    ///     Represents a consumer of access rights.
    /// </summary>
    internal class AccessRightsConsumer : IConsumer<IListAccessRights>,
                                          IConsumer<IAllowUserAccess>,
                                          IConsumer<IAllowUserGroupAccess>,
                                          IConsumer<IDenyUserAccess>,
                                          IConsumer<IDenyUserGroupAccess>,
                                          IConsumer<IScheduleUserAccess>,
                                          IConsumer<IScheduleUserGroupAccess>,
                                          IConsumer<IRemoveUserSchedule>,
                                          IConsumer<IRemoveGroupSchedule>
    {
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;
        private readonly IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> _findUserGroupRequest;
        private readonly IRequestClient<IListUsersInGroup, IListUsersInGroupResult> _listUsersInGroupRequest;
        private readonly IAccessRightsManager _accessRightsManager;
        private readonly IDatabaseContext _databaseContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessRightsConsumer" /> class.
        /// </summary>
        /// <param name="findUserRequest">The find user request.</param>
        /// <param name="findUserGroupRequest">The find user group request.</param>
        /// <param name="listUsersInGroupRequest">The list users in group request.</param>
        /// <param name="accessRightsManager">The access rights service.</param>
        /// <param name="databaseContext">The database context.</param>
        public AccessRightsConsumer(IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest,
                                    IRequestClient<IFindUserGroupByName, IFindUserGroupByNameResult> findUserGroupRequest,
                                    IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest,
                                    IAccessRightsManager accessRightsManager,
                                    IDatabaseContext databaseContext)
        {
            Contract.Requires(findUserRequest != null);
            Contract.Requires(findUserGroupRequest != null);
            Contract.Requires(listUsersInGroupRequest != null);
            Contract.Requires(accessRightsManager != null);
            Contract.Requires(databaseContext != null);

            _findUserRequest = findUserRequest;
            _findUserGroupRequest = findUserGroupRequest;
            _listUsersInGroupRequest = listUsersInGroupRequest;
            _accessRightsManager = accessRightsManager;
            _databaseContext = databaseContext;
        }

        /// <summary>
        ///     Allows access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAllowUserAccess> context)
        {
            var strategy = new PermanentUserAccessStrategy(_databaseContext, _findUserRequest, context.Message.UserName);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Allows access to the access point for the specified user group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAllowUserGroupAccess> context)
        {
            var strategy = new PermanentGroupAccessStrategy(_databaseContext, _findUserGroupRequest, _listUsersInGroupRequest, context.Message.UserGroupName);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Denies access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IDenyUserAccess> context)
        {
            var strategy = new PermanentUserAccessStrategy(_databaseContext, _findUserRequest, context.Message.UserName);
            var response = await _accessRightsManager.DenyAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Denies access to the access point for the specified user group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IDenyUserGroupAccess> context)
        {
            var strategy = new PermanentGroupAccessStrategy(_databaseContext, _findUserGroupRequest, _listUsersInGroupRequest, context.Message.UserGroupName);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }


        /// <summary>
        ///     Schedules user access.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IScheduleUserAccess> context)
        {
            var strategy = new ScheduledUserAccessStrategy(_databaseContext, _findUserRequest, context.Message.UserName, context.Message.WeeklySchedule);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Schedules user group access.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IScheduleUserGroupAccess> context)
        {
            var strategy = new ScheduledGroupAccessStrategy(_databaseContext, _findUserGroupRequest, _listUsersInGroupRequest, context.Message.UserGroupName, context.Message.WeeklySchedule);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Removes weekly scheduler for the user and access point.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IRemoveUserSchedule> context)
        {
            var strategy = new ScheduledUserAccessStrategy(_databaseContext, _findUserRequest, context.Message.UserName);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Removes weekly scheduler for the user group and access point.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IRemoveGroupSchedule> context)
        {
            var strategy = new ScheduledGroupAccessStrategy(_databaseContext, _findUserGroupRequest, _listUsersInGroupRequest, context.Message.UserGroupName);
            var response = await _accessRightsManager.AllowAccess(context.Message.AccessPointId, strategy);
            await context.RespondAsync(response);
        }

        /// <summary>
        ///     Lists access rights.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListAccessRights> context)
        {
            var accessPointFetcher = RoleBasedDataFetcher.Create(
                _databaseContext.AccessPoints.GetAll,
                manager => _databaseContext.AccessPoints.Filter(x => x.ManagedBy == manager));
            var accessPoints = accessPointFetcher.Execute();
            var accessPointIds = accessPoints.Select(x => x.AccessPointId).ToArray();
            var accessRights = _databaseContext.AccessRights.Filter(x => x.AccessRules.Any(rule => accessPointIds.Contains(rule.AccessPoint.AccessPointId)));

            var visitor = new ConvertAccessRightsVisitor();
            accessRights.ForEach(x => x.Accept(visitor));
            return context.RespondAsync(ListCommand.AccessRightsResult(visitor.UserAccessRightsDto.ToArray(), visitor.UserGroupAccessRightsDto.ToArray()));
        }
    }
}