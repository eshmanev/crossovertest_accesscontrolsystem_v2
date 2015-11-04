using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
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

        public AccessRightsConsumer(IRepository<Data.Entities.AccessPoint> accessPointRepository)
        {
            Contract.Requires(accessPointRepository != null);
            _accessPointRepository = accessPointRepository;
        }

        public Task Consume(ConsumeContext<IListAccessRights> context)
        {
            throw new System.NotImplementedException();
        }

        public Task Consume(ConsumeContext<IAllowUserAccess> context)
        {
            var accessPoint = _accessPointRepository.GetById(context.Message.AccessPointId);
            if (accessPoint == null)
                return context.RespondAsync(new VoidResult($"Access point {context.Message.AccessPointId} is not registered."));

            throw new NotImplementedException();
        }

        public Task Consume(ConsumeContext<IAllowUserGroupAccess> context)
        {
            var accessPoint = _accessPointRepository.GetById(context.Message.AccessPointId);
            if (accessPoint == null)
                return context.RespondAsync(new VoidResult($"Access point {context.Message.AccessPointId} is not registered."));

            throw new NotImplementedException();
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
    }
}