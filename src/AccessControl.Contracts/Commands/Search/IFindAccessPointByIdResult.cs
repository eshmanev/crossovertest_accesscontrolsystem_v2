using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Provides a result of the <see cref="IFindAccessPointById" /> command.
    /// </summary>
    [ContractClass(typeof(IFindAccessPointByIdResultContract))]
    public interface IFindAccessPointByIdResult
    {
        /// <summary>
        ///     Gets the access point.
        /// </summary>
        /// <value>
        ///     The access point.
        /// </value>
        IAccessPoint AccessPoint { get; }
    }
}