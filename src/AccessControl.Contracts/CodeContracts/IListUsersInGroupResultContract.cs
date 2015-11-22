using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListUsersInGroupResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListUsersInGroupResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListUsersInGroupResultContract : IListUsersInGroupResult
    {
        public IUser[] Users
        {
            get
            {
                Contract.Ensures(Contract.Result<IUser[]>() != null);
                return null;
            }
        }
    }
}