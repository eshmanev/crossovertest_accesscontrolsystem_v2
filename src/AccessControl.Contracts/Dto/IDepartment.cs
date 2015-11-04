using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    [ContractClass(typeof(IDepartmentContract))]
    public interface IDepartment
    {
        /// <summary>
        /// Gets the site.
        /// </summary>
        string SiteName { get; }

        /// <summary>
        /// Gets the site distinguished name.
        /// </summary>
        string Site { get; }

        /// <summary>
        /// Gets the department's name.
        /// </summary>
        string DepartmentName { get; }
    }
}