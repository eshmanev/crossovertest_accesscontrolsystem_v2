using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUpdateUserBiometric" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUpdateUserBiometric))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUpdateUserBiometricContract : IUpdateUserBiometric
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