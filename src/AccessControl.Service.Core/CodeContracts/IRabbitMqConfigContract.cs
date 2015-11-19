using System.Diagnostics.Contracts;
using AccessControl.Service.Configuration;

namespace AccessControl.Service.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IRabbitMqConfig" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRabbitMqConfig))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IRabbitMqConfigContract : IRabbitMqConfig
    {
        public string Url
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string GetQueueUrl(string queueName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(queueName));
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
            return null;
        }

        public string Password
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}