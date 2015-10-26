using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserExtended" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserExtended))]
    // ReSharper disable once InconsistentNaming
    internal abstract class UserExtendedContract : UserContract, IUserExtended
    {
        public string BiometricHash => null;
    }
}