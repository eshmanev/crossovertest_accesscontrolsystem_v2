using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IGetPasswordHash" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IGetPasswordHash))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IGetPasswordHashContract : IGetPasswordHash
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}