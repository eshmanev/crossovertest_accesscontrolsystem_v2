using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserBiometric" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserBiometric))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserBiometricContract : IUserContract, IUserBiometric
    {
        public string BiometricHash => null;
    }
}