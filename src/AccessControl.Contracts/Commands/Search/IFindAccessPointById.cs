using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Searches for an access point by its identifier.
    /// </summary>
    [ContractClass(typeof(IFindAccessPointByIdContract))]
    public interface IFindAccessPointById
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }
    }
}