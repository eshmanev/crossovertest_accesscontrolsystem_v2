using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUserByName" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUserByName))]
    // ReSharper disable once InconsistentNaming
    internal abstract class FindUserByNameContract : IFindUserByName
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