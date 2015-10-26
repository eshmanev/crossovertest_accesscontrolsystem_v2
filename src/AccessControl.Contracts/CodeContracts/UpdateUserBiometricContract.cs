using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUpdateUserBiometric" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUpdateUserBiometric))]
    // ReSharper disable once InconsistentNaming
    internal abstract class UpdateUserBiometricContract : IUpdateUserBiometric
    {
        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string BiometricHash => null;
    }
}