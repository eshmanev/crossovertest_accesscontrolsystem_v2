using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands
{
    /// <summary>
    ///     This command checks if the specified department exists.
    /// </summary>
    [ContractClass(typeof(IValidateDepartmentContract))]
    public interface IValidateDepartment
    {
        string Department { get; }
        string Site { get; }
    }
}