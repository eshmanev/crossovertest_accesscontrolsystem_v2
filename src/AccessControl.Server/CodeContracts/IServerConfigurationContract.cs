using System.Diagnostics.Contracts;
using AccessControl.Server.Configuration;

namespace AccessControl.Server.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IServerConfiguration" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IServerConfiguration))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IServerConfigurationContract : IServerConfiguration
    {
        public IRabbitMqSection RabbitMq
        {
            get
            {
                Contract.Ensures(Contract.Result<IRabbitMqSection>() != null);
                return null;
            }
        }
    }
}