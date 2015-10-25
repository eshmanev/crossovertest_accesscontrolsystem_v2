

namespace AccessControl.Contracts.CodeContracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts;

    /// <summary>
    /// Represents a contract class for the <see cref="IGetPasswordHashResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetPasswordHashResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetPasswordHashResultContract : IGetPasswordHashResult
    {
        public string PasswordHash => null;
    }
}