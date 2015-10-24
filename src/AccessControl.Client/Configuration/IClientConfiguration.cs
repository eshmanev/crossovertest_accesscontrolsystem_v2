using System.Diagnostics.Contracts;
using AccessControl.Client.CodeContracts;

namespace AccessControl.Client.Configuration
{
    [ContractClass(typeof(IClientConfigurationContract))]
    public interface IClientConfiguration
    {
        IRabbitMqSection RabbitMq { get; }
    }
}