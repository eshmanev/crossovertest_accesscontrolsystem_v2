using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(FindUserByNameContract))]
    public interface IFindUserByName
    {
        string UserName { get; }
    }
}