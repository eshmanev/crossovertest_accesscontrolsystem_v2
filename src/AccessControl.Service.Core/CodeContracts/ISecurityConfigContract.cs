using System.Diagnostics.Contracts;
using AccessControl.Service.Configuration;

namespace AccessControl.Service.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ISecurityConfig" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ISecurityConfig))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ISecurityConfigContract : ISecurityConfig
    {
        public string Secret
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}