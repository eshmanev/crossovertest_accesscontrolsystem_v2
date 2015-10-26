using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(UserWithBiometricContract))]
    public interface IUserWithBiometric : IUser
    {
        string BiometricHash { get; }
    }
}