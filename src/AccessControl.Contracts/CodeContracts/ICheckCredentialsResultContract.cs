using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ICheckCredentialsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ICheckCredentialsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ICheckCredentialsResultContract : ICheckCredentialsResult
    {
        public IUser User => null;
        public bool Valid => false;
    }
}