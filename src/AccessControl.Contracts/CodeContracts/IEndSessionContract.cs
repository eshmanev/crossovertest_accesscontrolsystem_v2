using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Synchronization;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IEndSession" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IEndSession))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IEndSessionContract : IEndSession
    {
    }
}