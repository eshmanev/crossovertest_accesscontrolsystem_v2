using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(IGetPasswordHashContract))]
    public interface IGetPasswordHash
    {
        string UserName { get; }
    }
}