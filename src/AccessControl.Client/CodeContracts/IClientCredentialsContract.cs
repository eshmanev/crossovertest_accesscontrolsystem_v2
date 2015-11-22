using System.Diagnostics.Contracts;
using AccessControl.Client.Services;

namespace AccessControl.Client.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IClientCredentials" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IClientCredentials))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IClientCredentialsContract : IClientCredentials
    {
        public string LdapPassword
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        public string LdapUserName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }
    }
}