

namespace AccessControl.Contracts.CodeContracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts;

    /// <summary>
    /// Represents a contract class for the <see cref="IAuthenticateUserResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAuthenticateUserResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class AuthenticateUserResultContract : IAuthenticateUserResult
    {
        public bool Authenticated => false;

        public string Message => null;
    }
}