using System.Diagnostics.Contracts;
using AccessControl.Client.Configuration;

namespace AccessControl.Client.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IClientConfiguration" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IClientConfiguration))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IClientConfigurationContract : IClientConfiguration
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