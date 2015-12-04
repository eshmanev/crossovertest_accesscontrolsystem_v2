using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDepartment" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDepartment))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IDepartmentContract : IDepartment
    {
        public string DepartmentName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}