using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserBiometricUpdated" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserBiometricUpdated))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserBiometricUpdatedContract : IUserBiometricUpdated
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string OldBiometricHash => null;
        public string NewBiometricHash => null;
    }
}