using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindAccessPointByIdResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindAccessPointByIdResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindAccessPointByIdResultContract : IFindAccessPointByIdResult
    {
        public IAccessPoint AccessPoint => null;
    }
}