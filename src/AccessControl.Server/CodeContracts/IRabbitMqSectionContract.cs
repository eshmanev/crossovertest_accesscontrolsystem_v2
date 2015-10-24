using System.Diagnostics.Contracts;
using AccessControl.Server.Configuration;

namespace AccessControl.Server.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IRabbitMqSection" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRabbitMqSection))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IRabbitMqSectionContract : IRabbitMqSection
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