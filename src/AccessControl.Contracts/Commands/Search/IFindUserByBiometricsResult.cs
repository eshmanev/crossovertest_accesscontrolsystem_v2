using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands.Search
{
    /// <summary>
    ///     Provides a result of the <see cref="IFindUserByBiometrics" /> command
    /// </summary>
    [ContractClass(typeof(IFindUserByBiometricsResultContract))]
    public interface IFindUserByBiometricsResult
    {
        /// <summary>
        ///     Gets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        IUserBiometric User { get; }
    }
}