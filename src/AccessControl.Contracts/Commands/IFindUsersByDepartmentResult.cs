using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(FindUsersByDepartmentResultContract))]
    public interface IFindUsersByDepartmentResult
    {
        IUser[] Users { get; }
    }
}