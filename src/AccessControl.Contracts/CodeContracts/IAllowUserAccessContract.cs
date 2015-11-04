using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAllowUserAccess" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAllowUserAccess))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAllowUserAccessContract : IAllowUserAccess
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return default(Guid);
            }
        }

        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}