using System.Diagnostics.Contracts;
using AccessControl.Service.CodeContracts;

namespace AccessControl.Service.Configuration
{
    [ContractClass(typeof(ISecurityConfigContract))]
    public interface ISecurityConfig
    {
        /// <summary>
        ///     Gets the secret for encryption.
        /// </summary>
        /// <value>
        ///     The secret.
        /// </value>
        string Secret { get; }
    }
}