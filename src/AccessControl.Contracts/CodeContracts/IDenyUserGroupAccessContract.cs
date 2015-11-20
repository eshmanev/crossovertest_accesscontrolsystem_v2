using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDenyUserGroupAccess" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDenyUserGroupAccess))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IDenyUserGroupAccessContract : IDenyUserGroupAccess
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