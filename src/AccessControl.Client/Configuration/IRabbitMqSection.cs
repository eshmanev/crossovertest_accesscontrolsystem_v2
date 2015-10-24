using System.Diagnostics.Contracts;
using AccessControl.Client.CodeContracts;

namespace AccessControl.Client.Configuration
{
    [ContractClass(typeof(IRabbitMqSectionContract))]
    public interface IRabbitMqSection
    {
        string Url { get; }
        string UserName { get; }
        string Password { get; }
    }
}