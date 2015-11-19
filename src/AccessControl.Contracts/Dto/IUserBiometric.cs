using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IUserBiometricContract))]
    public interface IUserBiometric : IUser
    {
        string BiometricHash { get; }
    }
}