using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListUserGroupsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListUserGroupsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListUserGroupsResultContract : IListUserGroupsResult
    {
        public IUserGroup[] Groups
        {
            get
            {
                Contract.Ensures(Contract.Result<IUserGroup[]>() != null);
                return null;
            }
        }
    }
}