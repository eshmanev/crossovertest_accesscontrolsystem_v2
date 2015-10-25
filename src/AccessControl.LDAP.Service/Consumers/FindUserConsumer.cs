using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using MassTransit;

namespace AccessControl.LDAP.Service.Consumers
{
    public class FindUserConsumer : IConsumer<IFindUserByName>
    {
        public Task Consume(ConsumeContext<IFindUserByName> context)
        {
            return context.RespondAsync(new FindUserByNameResult("hello"));
        }
    }
}