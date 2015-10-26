using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(IListBiometricInfoContract))]
    public interface IListBiometricInfo
    {
        string Department { get; }
        string Site { get; }
    }
}