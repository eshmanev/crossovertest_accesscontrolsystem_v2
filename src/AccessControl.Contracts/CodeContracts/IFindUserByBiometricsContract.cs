using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IFindUserByBiometrics" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IFindUserByBiometrics))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IFindUserByBiometricsContract : IFindUserByBiometrics
    {
        public string BiometricHash
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}