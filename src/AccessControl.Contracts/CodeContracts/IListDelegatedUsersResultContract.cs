using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListDelegatedUsersResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListDelegatedUsersResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListDelegatedUsersResultContract : IListDelegatedUsersResult
    {
        public string[] UserNames
        {
            get
            {
                Contract.Ensures(Contract.Result<string[]>() != null);
                return null;
            }
        }
    }
}