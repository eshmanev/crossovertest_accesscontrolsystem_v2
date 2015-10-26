using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUserByNameResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUserByNameResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindUserByNameResultContract : IFindUserByNameResult
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string DisplayName => null;
        public string Email => null;
        public string PhoneNumber => null;
    }
}