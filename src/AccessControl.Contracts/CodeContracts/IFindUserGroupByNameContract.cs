using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUserGroupByName" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUserGroupByName))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindUserGroupByNameContract : IFindUserGroupByName
    {
        public string UserGroup
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}