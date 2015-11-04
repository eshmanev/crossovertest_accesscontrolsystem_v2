using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    /// Represents a result of the <see cref="IListDepartments"/> command.
    /// </summary>
    [ContractClass(typeof(IListDepartmentsResultContract))]
    public interface IListDepartmentsResult
    {
        /// <summary>
        /// Gets an array of departments.
        /// </summary>
        IDepartment[] Departments { get; }
    }
}