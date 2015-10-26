using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    [ContractClass(typeof(FindUsersByDepartmentContract))]
    public interface IFindUsersByDepartment
    {
        string Site { get; }
        string Department { get; }
    }
}