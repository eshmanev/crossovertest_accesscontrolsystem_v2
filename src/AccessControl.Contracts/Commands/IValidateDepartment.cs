using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    ///     Checks if the specified department exists.
    /// </summary>
    [ContractClass(typeof(IValidateDepartmentContract))]
    public interface IValidateDepartment
    {
        /// <summary>
        ///     Gets the department.
        /// </summary>
        /// <value>
        ///     The department.
        /// </value>
        string Department { get; }

        /// <summary>
        ///     Gets the site.
        /// </summary>
        /// <value>
        ///     The site.
        /// </value>
        string Site { get; }
    }
}