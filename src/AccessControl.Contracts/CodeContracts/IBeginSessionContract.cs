using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IBeginSession" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IBeginSession))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IBeginSessionContract : IBeginSession
    {
    }
}