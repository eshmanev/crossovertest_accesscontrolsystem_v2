using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListAccessRights" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListAccessRights))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListAccessRightsContract : IListAccessRights
    {
    }
}