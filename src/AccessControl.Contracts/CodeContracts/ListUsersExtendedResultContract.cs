using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListUsersExtendedResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListUsersExtendedResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ListUsersExtendedResultContract : IListUsersExtendedResult
    {
        public IUserExtended[] Users
        {
            get
            {
                Contract.Ensures(Contract.Result<IUserExtended[]>() != null);
                return null;
            }
        }
    }
}