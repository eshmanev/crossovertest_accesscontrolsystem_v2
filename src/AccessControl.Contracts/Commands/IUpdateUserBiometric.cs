using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(UpdateUserBiometricContract))]
    public interface IUpdateUserBiometric
    {
        string UserName { get; }
        string BiometricHash { get; }
    }
}