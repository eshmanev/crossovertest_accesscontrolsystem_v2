using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindAccessPointById" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindAccessPointById))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindAccessPointByIdContract : IFindAccessPointById
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