using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IPermanentAccessRule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IPermanentAccessRule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IPermanentAccessRuleContract : IPermanentAccessRule
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }
    }
}