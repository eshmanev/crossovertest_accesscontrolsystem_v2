using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAuthenticateUserResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAuthenticateUserResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAuthenticateUserResultContract : IAuthenticateUserResult
    {
        public bool Authenticated => false;
        public string Message => null;
    }
}