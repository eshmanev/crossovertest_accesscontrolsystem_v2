using System.Diagnostics.Contracts;
using AccessControl.Service.Core.CodeContracts;

namespace AccessControl.Service.Core.Configuration
{
    [ContractClass(typeof(IRabbitMqConfigContract))]
    public interface IRabbitMqConfig
    {
        string Url { get; }
        string UserName { get; }
        string Password { get; }
    }
}