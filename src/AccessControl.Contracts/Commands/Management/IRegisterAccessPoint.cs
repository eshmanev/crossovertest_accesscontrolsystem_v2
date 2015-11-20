using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Registers the given access point.
    /// </summary>
    [ContractClass(typeof(IRegisterAccessPointContract))]
    public interface IRegisterAccessPoint
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