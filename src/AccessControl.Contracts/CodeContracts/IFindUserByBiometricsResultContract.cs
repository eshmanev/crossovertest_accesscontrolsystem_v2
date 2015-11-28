using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUserByBiometricsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUserByBiometricsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindUserByBiometricsResultContract : IFindUserByBiometricsResult
    {
        public IUserBiometric User => null;
    }
}