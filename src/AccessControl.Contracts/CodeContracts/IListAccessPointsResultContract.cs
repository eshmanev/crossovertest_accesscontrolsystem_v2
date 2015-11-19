using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListAccessPointsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListAccessPointsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListAccessPointsResultContract : IListAccessPointsResult
    {
        public IAccessPoint[] AccessPoints
        {
            get
            {
                Contract.Ensures(Contract.Result<IAccessPoint[]>() != null);
                return null;
            }
        }
    }
}