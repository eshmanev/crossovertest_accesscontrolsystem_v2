using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using MassTransit;

namespace AccessControl.Service.Consumers
{
    internal class PingConsumer : IConsumer<Ping>
    {
        public Task Consume(ConsumeContext<Ping> context)
        {
            return context.RespondAsync(context.Message);
        }
    }
}