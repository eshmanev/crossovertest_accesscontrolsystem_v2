﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class DelegateConsumer : IConsumer<IListDelegatedUsers>, IConsumer<IGrantManagementRights>, IConsumer<IRevokeManagementRights>
    {
        private readonly IRepository<DelegatedRights> _delegatedRightsRepository;
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegateConsumer" /> class.
        /// </summary>
        /// <param name="delegatedRightsRepository">The delegated rights repository.</param>
        /// <param name="findUserRequest">The find user request.</param>
        public DelegateConsumer(IRepository<DelegatedRights> delegatedRightsRepository,
                                IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest)
        {
            Contract.Requires(delegatedRightsRepository != null);
            Contract.Requires(findUserRequest != null);

            _delegatedRightsRepository = delegatedRightsRepository;
            _findUserRequest = findUserRequest;
        }

        /// <summary>
        ///     Grants management rights for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IGrantManagementRights> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                await context.RespondAsync(new VoidResult("Not authorized"));
                return;
            }

            var userResult = await _findUserRequest.Request(new FindUserByName(context.Message.UserName));
            if (userResult.User == null)
            {
                await context.RespondAsync(new VoidResult("User cannot be found"));
                return;
            }

            var entity = _delegatedRightsRepository
                .Filter(x => x.Grantor == context.UserName() && x.Grantee == context.Message.UserName)
                .SingleOrDefault();

            if (entity != null)
            {
                await context.RespondAsync(new VoidResult());
                return;
            }

            entity = new DelegatedRights {Grantor = context.UserName(), Grantee = userResult.User.UserName};
            _delegatedRightsRepository.Insert(entity);
            await context.RespondAsync(new VoidResult());
        }

        /// <summary>
        ///     Lists the delegated users.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task Consume(ConsumeContext<IListDelegatedUsers> context)
        {
            IEnumerable<string> userNames;
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                userNames = Enumerable.Empty<string>();
            }
            else
            {
                userNames = _delegatedRightsRepository.Filter(x => x.Grantor == context.UserName()).Select(x => x.Grantee);
            }

            return context.RespondAsync(ListCommand.DelegatedUsersResult(userNames.ToArray()));
        }

        /// <summary>
        ///     Revokes management rights for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IRevokeManagementRights> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                return context.RespondAsync(new VoidResult("Not authorized"));
            }

            var entity = _delegatedRightsRepository
                .Filter(x => x.Grantor == context.UserName() && x.Grantee == context.Message.UserName)
                .SingleOrDefault();

            if (entity != null)
            {
                _delegatedRightsRepository.Delete(entity);
            }

            return context.RespondAsync(new VoidResult());
        }
    }
}