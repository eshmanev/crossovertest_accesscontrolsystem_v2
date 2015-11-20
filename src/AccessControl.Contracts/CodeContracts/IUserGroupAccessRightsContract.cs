using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserGroupAccessRights" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserGroupAccessRights))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserGroupAccessRightsContract : IUserGroupAccessRights
    {
        public string UserGroupName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public IPermanentAccessRule[] PermanentAccessRules
        {
            get
            {
                Contract.Ensures(Contract.Result<IPermanentAccessRule[]>() != null);
                return null;
            }
        }
    }
}