using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Server.Data;
using AccessControl.Server.Data.Entities;
using MassTransit;

namespace AccessControl.Server.Consumers
{
    public class RegisterAccessPointConsumer : IConsumer<IRegisterAccessPoint>
    {
        private readonly IRepository<AccessPoint> _accessPointRepository;

        public RegisterAccessPointConsumer(IRepository<AccessPoint> accessPointRepository)
        {
            Contract.Requires(accessPointRepository != null);
            _accessPointRepository = accessPointRepository;
        }

        public Task Consume(ConsumeContext<IRegisterAccessPoint> context)
        {
            var accessPoint = new AccessPoint
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