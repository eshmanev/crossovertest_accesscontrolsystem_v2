using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(FindUserByNameContract))]
    public interface IFindUserByName
    {
        string UserName { get; }
    }
}