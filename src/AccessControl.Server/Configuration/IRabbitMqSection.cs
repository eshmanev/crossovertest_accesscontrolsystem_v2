using System.Diagnostics.Contracts;
using AccessControl.Server.CodeContracts;

namespace AccessControl.Server.Configuration
{
    [ContractClass(typeof(IRabbitMqSectionContract))]
    public interface IRabbitMqSection
    {
        string Url { get; }
        string UserName { get; }
        string Password { get; }
    }
}