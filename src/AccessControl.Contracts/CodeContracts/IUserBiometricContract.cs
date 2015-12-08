using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IUserBiometric" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IUserBiometric))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IUserBiometricContract : IUserBiometric
    {
        public string BiometricHash => null;

        string IUser.Department => null;

        string IUser.DisplayName => null;

        string IUser.Email => null;

        IUserGroup[] IUser.Groups => null;

        bool IUser.IsManager => false;

        string IUser.ManagerName => null;

        string IUser.PhoneNumber => null;

        string IUser.UserName => null;
    }
}