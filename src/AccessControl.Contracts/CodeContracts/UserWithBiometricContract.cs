using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserWithBiometric" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserWithBiometric))]
    // ReSharper disable once InconsistentNaming
    internal abstract class UserWithBiometricContract : UserContract, IUserWithBiometric
    {
        public string BiometricHash => null;
    }
}