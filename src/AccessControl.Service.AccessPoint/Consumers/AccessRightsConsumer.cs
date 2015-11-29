﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using AccessControl.Data.Entities;
using AccessControl.Service.AccessPoint.Helpers;
using AccessControl.Service.Security;
using MassTransit;
using Microsoft.Practices.ObjectBuilder2;

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
        private readonly IBus _bus;
        private readonly IRequestClient<IListUsersInGroup, IListUsersInGroupResult> _listUsersInGroupRequest;
        private readonly IDatabaseContext _databaseContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessRightsConsumer" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="listUsersInGroupRequest">The list users in group request.</param>
        /// <param name="databaseContext">The database context.</param>
        public AccessRightsConsumer(IBus bus,
                                    IRequestClient<IListUsersInGroup, IListUsersInGroupResult> listUsersInGroupRequest,
                                    IDatabaseContext databaseContext)
        {
            Contract.Requires(bus != null);
            Contract.Requires(listUsersInGroupRequest != null);
            Contract.Requires(databaseContext != null);

            _bus = bus;
            _listUsersInGroupRequest = listUsersInGroupRequest;
            _databaseContext = databaseContext;
        }

        /// <summary>
        ///     Allows access to the access point for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IAllowUserAccess> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                return context.RespondAsync(new VoidResult("Not authorized"));
            }

            var accessRights = GetUserAccessRights(context.Message.UserName) ?? new UserAccessRights {UserName = context.Message.UserName};
            IVoidResult response;
            if (TryAllowPermanentAccess(context.Message.AccessPointId, accessRights, out response))
            {
                var hash = FindUserHash(context.Message.UserName);
                _bus.Publish(new PermanentUserAccessAllowed(context.Message.AccessPointId, context.Message.UserName, hash));
            }
            return context.RespondAsync(response);
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
            IVoidResult response;
            if (TryAllowPermanentAccess(context.Message.AccessPointId, accessRights, out response))
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
            await context.RespondAsync(response);
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
            IVoidResult response;
            if (TryDenyPermanentAccess(context.Message.AccessPointId, accessRights, out response))
            {
                _bus.Publish(new PermanentUserAccessDenied(context.Message.AccessPointId, context.Message.UserName));
            }
            return context.RespondAsync(response);
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
            IVoidResult response;
            if (TryDenyPermanentAccess(context.Message.AccessPointId, accessRights, out response))
            {
                _bus.Publish(new PermanentUserGroupAccessDenied(context.Message.AccessPointId, context.Message.UserGroupName));
            }
            return context.RespondAsync(response);
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

        private string FindUserHash(string userName)
        {
            var hashEntity = _databaseContext.Users.Filter(x => x.UserName == userName).SingleOrDefault();
            return hashEntity?.BiometricHash;
        }

        private UserAccessRights GetUserAccessRights(string userName)
        {
            return _databaseContext.AccessRights.Filter(x => x is UserAccessRights && ((UserAccessRights) x).UserName == userName)
                                          .Cast<UserAccessRights>()
                                          .FirstOrDefault();
        }

        private UserGroupAccessRights GetUserGroupAccessRights(string userGroupName)
        {
            return _databaseContext.AccessRights.Filter(x => x is UserGroupAccessRights && ((UserGroupAccessRights) x).UserGroupName == userGroupName)
                                          .Cast<UserGroupAccessRights>()
                                          .FirstOrDefault();
        }

        private bool TryAllowPermanentAccess(Guid accessPointId, AccessRightsBase accessRights, out IVoidResult response)
        {
            if (accessRights.AccessRules.OfType<PermanentAccessRule>().Any(x => x.AccessPoint.AccessPointId == accessPointId))
            {
                response = new VoidResult();
                return false;
            }

            var accessPoint = _databaseContext.AccessPoints.GetById(accessPointId);
            if (accessPoint == null)
            {
                response = new VoidResult($"Access point {accessPointId} is not registered.");
                return false;
            }

            accessRights.AddAccessRule(new PermanentAccessRule {AccessPoint = accessPoint});
            if (accessRights.Id == 0)
            {
                _databaseContext.AccessRights.Insert(accessRights);
            }
            else
            {
                _databaseContext.AccessRights.Update(accessRights);
            }
            _databaseContext.Commit();

            response = new VoidResult();
            return true;
        }

        private bool TryDenyPermanentAccess(Guid accessPointId, AccessRightsBase accessRights, out IVoidResult response)
        {
            if (accessRights == null)
            {
                response = new VoidResult();
                return false;
            }

            var accessRule = accessRights.AccessRules
                                         .OfType<PermanentAccessRule>()
                                         .FirstOrDefault(x => x.AccessPoint.AccessPointId == accessPointId);

            if (accessRule == null)
            {
                response = new VoidResult();
                return false;
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

            response = new VoidResult();
            return true;
        }
    }
}