using System.Threading.Tasks;
using AccessControl.Contracts.Events;
using MassTransit;

namespace AccessControl.Service.Notifications.Consumers
{
    public class AccessConsumer : IConsumer<IAccessAttempted>
    {
        public Task Consume(ConsumeContext<IAccessAttempted> context)
        {
            return Task.FromResult(true);
        }
    }
}