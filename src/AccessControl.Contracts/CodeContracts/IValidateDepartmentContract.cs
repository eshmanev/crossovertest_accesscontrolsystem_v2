using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IValidateDepartment" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IValidateDepartment))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IValidateDepartmentContract : IValidateDepartment
    {
        public string Department
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public string Site
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}