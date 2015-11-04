using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    /// Execute this command when you need to get a list of departments.
    /// </summary>
    [ContractClass(typeof(IListDepartmentsContract))]
    public interface IListDepartments
    {
    }
}