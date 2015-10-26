using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Data;
using AccessControl.Data.Entities;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class RegisterAccessPointConsumer : IConsumer<IRegisterAccessPoint>
    {
        private readonly IRepository<Data.Entities.AccessPoint> _accessPointRepository;

        public RegisterAccessPointConsumer(IRepository<Data.Entities.AccessPoint> accessPointRepository)
        {
            Contract.Requires(accessPointRepository != null);
            _accessPointRepository = accessPointRepository;
        }

        public Task Consume(ConsumeContext<IRegisterAccessPoint> context)
        {
            var accessPoint = new Data.Entities.AccessPoint
            {
                AccessPointId = context.Message.AccessPointId,
                Name = context.Message.Name,
                Description = context.Message.Description
            };
            _accessPointRepository.Insert(accessPoint);
            return Task.FromResult(true);
        }
    }
}