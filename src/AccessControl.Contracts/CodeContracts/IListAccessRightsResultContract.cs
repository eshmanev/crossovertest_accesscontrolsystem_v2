using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListAccessRightsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListAccessRightsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListAccessRightsResultContract : IListAccessRightsResult
    {
    }
}