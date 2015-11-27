using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Helpers;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using AccessControl.Service.AccessPoint.Visitors;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;
using User = AccessControl.Data.Entities.User;

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
        private readonly IBus _bus;
        private readonly IRequestClient<IListUsersInGroup, IListUsersInGroupResult> _listUsersInGroupRequest;
        private readonly IRepository<User> _userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessRightsConsumer" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="listUsersInGroupRequest">The list users in group request.</param>
        /// <param name="accessPointRepository">The access point repository.</param>
        /// <param name="accessRightsRepository">The access rights repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public AccessRightsConsumer(IBus bus,
                                    IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest,
                                    IRepository<Data.Entities.AccessPoint> accessPointRepository,
                                    IRepository<AccessRightsBase> accessRightsRepository,
                                    IRepository<User> userRepository)
        {
            Contract.Requires(bus != null);
            Contract.Requires(listUsersInGroupRequest != null);
            Contract.Requires(accessPointRepository != null);
            Contract.Requires(accessRightsRepository != null);
            Contract.Requires(userRepository != null);

            _bus = bus;
            _listUsersInGroupRequest = listUsersInGroupRequest;
            _accessPointRepository = accessPointRepository;
            _accessRightsRepository = accessRightsRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        ///     Allows access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IAllowUserAccess> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
                return context.RespondAsync(new VoidResult("Not authorized"));

            var accessRights = GetUserAccessRights(context.Message.UserName) ?? new UserAccessRights {UserName = context.Message.UserName};
            Task response;
            if (TryAllowPermanentAccess(context, context.Message.AccessPointId, accessRights, out response))
            {
                var hash = FindUserHash(context.Message.UserName);
                _bus.Publish(new PermanentUserAccessAllowed(context.Message.AccessPointId, context.Message.UserName, hash));
            }
            return response;
        }

        /// <summary>
        ///     Allows access to the access point for the specified user group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IAllowUserGroupAccess> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                await context.RespondAsync(new VoidResult("Not authorized"));
                return;
            }

            var accessRights = GetUserGroupAccessRights(context.Message.UserGroupName) ?? new UserGroupAccessRights {UserGroupName = context.Message.UserGroupName};
            Task response;
            if (TryAllowPermanentAccess(context, context.Message.AccessPointId, accessRights, out response))
            {
                var usersInGroupResult = await _listUsersInGroupRequest.Request(ListCommand.ListUsersInGroup(context.Message.UserGroupName));
                var userNames = usersInGroupResult.Users.Select(x => x.UserName).ToArray();
                var userHashes = new string[userNames.Length];
                for (var i = 0; i < userNames.Length; i++)
                {
                    userHashes[i] = FindUserHash(userNames[i]);
                }

                await _bus.Publish(new PermanentUserGroupAccessAllowed(context.Message.AccessPointId, context.Message.UserGroupName, userNames, userHashes));
            }
            await response;
        }

        /// <summary>
        ///     Denies access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IDenyUserAccess> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                return context.RespondAsync(new VoidResult("Not authorized"));
            }

            var accessRights = GetUserAccessRights(context.Message.UserName);
            Task response;
            if (TryDenyPermanentAccess(context, context.Message.AccessPointId, accessRights, out response))
            {
                _bus.Publish(new PermanentUserAccessDenied(context.Message.AccessPointId, context.Message.UserName));
            }
            return response;
        }

        /// <summary>
        ///     Denies access to the access point for the specified user group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IDenyUserGroupAccess> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                return context.RespondAsync(new VoidResult("Not authorized"));
            }

            var accessRights = GetUserGroupAccessRights(context.Message.UserGroupName);
            Task response;
            if (TryDenyPermanentAccess(context, context.Message.AccessPointId, accessRights, out response))
            {
                _bus.Publish(new PermanentUserGroupAccessDenied(context.Message.AccessPointId, context.Message.UserGroupName));
            }
            return response;
        }

        /// <summary>
        ///     Lists access rights.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListAccessRights> context)
        {
            IEnumerable<Data.Entities.AccessPoint> accessPoints;

            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                accessPoints = _accessPointRepository.GetAll();
            }
            else if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                accessPoints = _accessPointRepository.Filter(x => x.Site == context.Site() && x.Department == context.Department());
            }
            else
            {
                accessPoints = Enumerable.Empty<Data.Entities.AccessPoint>();
            }

            var accessPointIds = accessPoints.Select(x => x.AccessPointId).ToArray();
            var accessRights = _accessRightsRepository.Filter(x => x.AccessRules.Any(rule => accessPointIds.Contains(rule.AccessPoint.AccessPointId)));

            var visitor = new ConvertAccessRightsVisitor();
            accessRights.ForEach(x => x.Accept(visitor));
            return context.RespondAsync(ListCommand.AccessRightsResult(visitor.UserAccessRightsDto.ToArray(), visitor.UserGroupAccessRightsDto.ToArray()));
        }

        

        private string FindUserHash(string userName)
        {
            var hashEntity = _userRepository.Filter(x => x.UserName == userName).SingleOrDefault();
            return hashEntity?.BiometricHash;
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

        private bool TryAllowPermanentAccess(ConsumeContext context, Guid accessPointId, AccessRightsBase accessRights, out Task response)
        {
            if (accessRights.AccessRules.OfType<PermanentAccessRule>().Any(x => x.AccessPoint.AccessPointId == accessPointId))
            {
                response = context.RespondAsync(new VoidResult());
                return false;
            }

            var accessPoint = _accessPointRepository.GetById(accessPointId);
            if (accessPoint == null)
            {
                response = context.RespondAsync(new VoidResult($"Access point {accessPointId} is not registered."));
                return false;
            }

            accessRights.AddAccessRule(new PermanentAccessRule {AccessPoint = accessPoint});
            if (accessRights.Id == 0)
            {
                _accessRightsRepository.Insert(accessRights);
            }
            else
            {
                _accessRightsRepository.Update(accessRights);
            }

            response = context.RespondAsync(new VoidResult());
            return true;
        }

        private bool TryDenyPermanentAccess(ConsumeContext context, Guid accessPointId, AccessRightsBase accessRights, out Task response)
        {
            if (accessRights == null)
            {
                response = context.RespondAsync(new VoidResult());
                return false;
            }

            var accessRule = accessRights.AccessRules
                                         .OfType<PermanentAccessRule>()
                                         .FirstOrDefault(x => x.AccessPoint.AccessPointId == accessPointId);

            if (accessRule == null)
            {
                response = context.RespondAsync(new VoidResult());
                return false;
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

            response = context.RespondAsync(new VoidResult());
            return true;
        }
    }
}