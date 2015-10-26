using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(UpdateUserBiometricContract))]
    public interface IUpdateUserBiometric
    {
        string UserName { get; }
        string BiometricHash { get; }
    }
}