using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts
{
    [ContractClass(typeof(FindUsersByDepartmentContract))]
    public interface IFindUsersByDepartment
    {
        string Site { get; }
        string Department { get; }
    }
}