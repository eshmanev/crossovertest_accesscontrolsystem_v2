using System.Diagnostics.Contracts;
using AccessControl.Server.CodeContracts;

namespace AccessControl.Server.Configuration
{
    [ContractClass(typeof(IServerConfigurationContract))]
    public interface IServerConfiguration
    {
        IRabbitMqSection RabbitMq { get; }
    }
}