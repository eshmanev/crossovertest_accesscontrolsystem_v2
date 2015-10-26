using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAuthenticateUser" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAuthenticateUser))]
    // ReSharper disable once InconsistentNaming
    internal abstract class AuthenticateUserContract : IAuthenticateUser
    {
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