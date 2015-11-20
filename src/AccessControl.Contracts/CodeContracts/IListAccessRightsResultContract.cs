using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListAccessRightsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListAccessRightsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListAccessRightsResultContract : IListAccessRightsResult
    {
        public IUserAccessRights[] UserAccessRights
        {
            get
            {
                Contract.Ensures(Contract.Result<IUserAccessRights[]>() != null);
                return null;
            }
        }

        public IUserGroupAccessRights[] UserGroupAccessRights
        {
            get
            {
                Contract.Ensures(Contract.Result<IUserGroupAccessRights[]>() != null);
                return null;
            }
        }
    }
}