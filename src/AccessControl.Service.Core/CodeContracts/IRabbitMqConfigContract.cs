using System.Diagnostics.Contracts;
using AccessControl.Service.Core.Configuration;

namespace AccessControl.Service.Core.CodeContracts
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