using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ICheckCredentials" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ICheckCredentials))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ICheckCredentialsContract : ICheckCredentials
    {
        public string Domain {
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