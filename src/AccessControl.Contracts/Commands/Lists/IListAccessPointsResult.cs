using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Lists
{
    /// <summary>
    ///     Represents a result of the <see cref="IListAccessPoints" /> command.
    /// </summary>
    [ContractClass(typeof(IListAccessPointsResultContract))]
    public interface IListAccessPointsResult
    {
        /// <summary>
        ///     Gets the access points.
        /// </summary>
        /// <value>
        ///     The access points.
        /// </value>
        IAccessPoint[] AccessPoints { get; }
    }
}