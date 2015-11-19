using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class AccessRightsConsumer : IConsumer<IListAccessRights>,
                                        IConsumer<IAllowUserAccess>,
                                        IConsumer<IAllowUserGroupAccess>,
                                        IConsumer<IDenyUserAccess>,
                                        IConsumer<IDenyUserGroupAccess>
    {
        private readonly IRepository<Data.Entities.AccessPoint> _accessPointRepository;
        private readonly IRepository<AccessRightsBase> _accessRightsRepository;

        public AccessRightsConsumer(IRepository<Data.Entities.AccessPoint> accessPointRepository,
                                    IRepository<AccessRightsBase> accessRightsRepository)
        {
            Contract.Requires(accessPointRepository != null);
            Contract.Requires(accessRightsRepository != null);

            _accessPointRepository = accessPointRepository;
            _accessRightsRepository = accessRightsRepository;
        }

        public Task Consume(ConsumeContext<IListAccessRights> context)
        {
            throw new NotImplementedException();
        }

        public Task Consume(ConsumeContext<IAllowUserAccess> context)
        {
            var accessRights = _accessRightsRepository.Filter(x => x is UserAccessRights && ((UserAccessRights) x).UserName == context.Message.UserName)
                                                      .Cast<UserAccessRights>()
                                                      .FirstOrDefault();
            return AllowPermanentAccess(
                context, 
                context.Message.AccessPointId, 
                accessRights ?? new UserAccessRights {UserName = context.Message.UserName});
        }

        public Task Consume(ConsumeContext<IAllowUserGroupAccess> context)
        {
            var accessRights = _accessRightsRepository.Filter(x => x is UserGroupAccessRights && ((UserGroupAccessRights) x).UserGroupName == context.Message.UserGroupName)
                                                      .Cast<UserGroupAccessRights>()
                                                      .FirstOrDefault();
            return AllowPermanentAccess(
                context,
                context.Message.AccessPointId,
                accessRights ?? new UserGroupAccessRights {UserGroupName = context.Message.UserGroupName});
        }

        public Task Consume(ConsumeContext<IDenyUserAccess> context)
        {
            var accessPoint = _accessPointRepository.GetById(context.Message.AccessPointId);
            if (accessPoint == null)
                return context.RespondAsync(new VoidResult($"Access point {context.Message.AccessPointId} is not registered."));

            throw new NotImplementedException();
        }

        public Task Consume(ConsumeContext<IDenyUserGroupAccess> context)
        {
            var accessPoint = _accessPointRepository.GetById(context.Message.AccessPointId);
            if (accessPoint == null)
                return context.RespondAsync(new VoidResult($"Access point {context.Message.AccessPointId} is not registered."));

            throw new NotImplementedException();
        }

        private Task AllowPermanentAccess(ConsumeContext context, Guid accessPointId, AccessRightsBase accessRights)
        {
            var accessPoint = _accessPointRepository.GetById(accessPointId);
            if (accessPoint == null)
                return context.RespondAsync(new VoidResult($"Access point {accessPointId} is not registered."));

            accessRights.AddAccessRule(new PermanentAccessRule { AccessPoint = accessPoint });
            if (accessRights.Id == 0)
                _accessRightsRepository.Insert(accessRights);
            else
                _accessRightsRepository.Update(accessRights);

            return context.RespondAsync(new VoidResult());
        }
    }
}