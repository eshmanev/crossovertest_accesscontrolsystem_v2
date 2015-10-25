using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using MassTransit;

namespace AccessControl.LDAP.Service.Consumers
{
    public class GetPasswordHashConsumer : IConsumer<IGetPasswordHash>
    {
        public Task Consume(ConsumeContext<IGetPasswordHash> context)
        {
            return context.RespondAsync(new GetPasswordHashResult(string.Empty));
        }
    }
}