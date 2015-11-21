using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using AccessControl.Data.Entities;
using AccessControl.Service.AccessPoint.Visitors;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    /// <summary>
    ///     Represents a consumer of access rights.
    /// </summary>
    public class AccessRightsConsumer : IConsumer<IListAccessRights>,
                                        IConsumer<IAllowUserAccess>,
                                        IConsumer<IAllowUserGroupAccess>,
                                        IConsumer<IDenyUserAccess>,
                                        IConsumer<IDenyUserGroupAccess>
    {
        private readonly IRepository<Data.Entities.AccessPoint> _accessPointRepository;
        private readonly IRepository<AccessRightsBase> _accessRightsRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessRightsConsumer" /> class.
        /// </summary>
        /// <param name="accessPointRepository">The access point repository.</param>
        /// <param name="accessRightsRepository">The access rights repository.</param>
        public AccessRightsConsumer(IRepository<Data.Entities.AccessPoint> accessPointRepository,
                                    IRepository<AccessRightsBase> accessRightsRepository)
        {
            Contract.Requires(accessPointRepository != null);
            Contract.Requires(accessRightsRepository != null);

            _accessPointRepository = accessPointRepository;
            _accessRightsRepository = accessRightsRepository;
        }

        /// <summary>
        ///     Allows access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IAllowUserAccess> context)
        {
            var accessRights = GetUserAccessRights(context.Message.UserName);
            return TryAllowPermanentAccess(
                context,
                context.Message.AccessPointId,
                accessRights ?? new UserAccessRights {UserName = context.Message.UserName});
        }

        /// <summary>
        ///     Allows access to the access point for the specified user group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IAllowUserGroupAccess> context)
        {
            var accessRights = GetUserGroupAccessRights(context.Message.UserGroupName);
            return TryAllowPermanentAccess(
                context,
                context.Message.AccessPointId,
                accessRights ?? new UserGroupAccessRights {UserGroupName = context.Message.UserGroupName});
        }

        /// <summary>
        ///     Denies access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IDenyUserAccess> context)
        {
            var accessRights = GetUserAccessRights(context.Message.UserName);
            return DenyPermanentAccess(context, context.Message.AccessPointId, accessRights);
        }

        /// <summary>
        ///     Denies access to the access point for the specified user group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IDenyUserGroupAccess> context)
        {
            var accessRights = GetUserGroupAccessRights(context.Message.UserGroupName);
            return DenyPermanentAccess(context, context.Message.AccessPointId, accessRights);
        }

        /// <summary>
        ///     Lists access rights.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListAccessRights> context)
        {
            var accessPoints = _accessPointRepository.Filter(x => x.Site == context.Site() && x.Department == context.Department());
            var accessPointIds = accessPoints.Select(x => x.AccessPointId).ToArray();
            var accessRights = _accessRightsRepository.Filter(x => x.AccessRules.Any(rule => accessPointIds.Contains(rule.AccessPoint.AccessPointId)));

            var visitor = new ConvertAccessRightsVisitor();
            foreach (var item in accessRights)
            {
                item.Accept(visitor);
            }

            return context.RespondAsync(ListCommand.Result(visitor.UserAccessRightsDto.ToArray(), visitor.UserGroupAccessRightsDto.ToArray()));
        }

        private Task DenyPermanentAccess(ConsumeContext context, Guid accessPointId, AccessRightsBase accessRights)
        {
            if (accessRights == null)
            {
                return context.RespondAsync(new VoidResult());
            }

            var accessRule = accessRights.AccessRules
                                         .OfType<PermanentAccessRule>()
                                         .FirstOrDefault(x => x.AccessPoint.AccessPointId == accessPointId);

            if (accessRule == null)
            {
                return context.RespondAsync(new VoidResult());
            }

            accessRights.RemoveAccessRule(accessRule);
            if (accessRights.AccessRules.Any())
            {
                _accessRightsRepository.Update(accessRights);
            }

            else
            {
                _accessRightsRepository.Delete(accessRights);
            }

            return context.RespondAsync(new VoidResult());
        }

        private UserAccessRights GetUserAccessRights(string userName)
        {
            return _accessRightsRepository.Filter(x => x is UserAccessRights && ((UserAccessRights) x).UserName == userName)
                                          .Cast<UserAccessRights>()
                                          .FirstOrDefault();
        }

        private UserGroupAccessRights GetUserGroupAccessRights(string userGroupName)
        {
            return _accessRightsRepository.Filter(x => x is UserGroupAccessRights && ((UserGroupAccessRights) x).UserGroupName == userGroupName)
                                          .Cast<UserGroupAccessRights>()
                                          .FirstOrDefault();
        }

        private Task TryAllowPermanentAccess(ConsumeContext context, Guid accessPointId, AccessRightsBase accessRights)
        {
            if (accessRights.AccessRules.Any(x => x.AccessPoint.AccessPointId == accessPointId))
            {
                return context.RespondAsync(new VoidResult());
            }

            var accessPoint = _accessPointRepository.GetById(accessPointId);
            if (accessPoint == null)
            {
                return context.RespondAsync(new VoidResult($"Access point {accessPointId} is not registered."));
            }

            accessRights.AddAccessRule(new PermanentAccessRule {AccessPoint = accessPoint});
            if (accessRights.Id == Guid.Empty)
            {
                _accessRightsRepository.Insert(accessRights);
            }
            else
            {
                _accessRightsRepository.Update(accessRights);
            }

            return context.RespondAsync(new VoidResult());
        }
    }
}