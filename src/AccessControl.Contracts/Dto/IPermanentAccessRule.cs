using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IPermanentAccessRuleContract))]
    public interface IPermanentAccessRule
    {
        /// <summary>
        /// Gets the access point identifier.
        /// </summary>
        /// <value>
        /// The access point identifier.
        /// </value>
        Guid AccessPointId { get; }
    }
}