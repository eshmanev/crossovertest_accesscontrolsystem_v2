using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAllowUserGroupAccess" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAllowUserGroupAccess))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAllowUserGroupAccessContract : IAllowUserGroupAccess
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return default(Guid);
            }
        }

        public string UserGroupName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}