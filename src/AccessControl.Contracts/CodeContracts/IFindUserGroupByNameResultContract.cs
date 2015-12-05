using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUserGroupByNameResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUserGroupByNameResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindUserGroupByNameResultContract : IFindUserGroupByNameResult
    {
        public IUserGroup UserGroup => null;
    }
}