using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IListDepartmentsResult" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListDepartmentsResult))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListDepartmentsResultContract : IListDepartmentsResult
    {
        public IDepartment[] Departments
        {
            get
            {
                Contract.Ensures(Contract.Result<IDepartment[]>() != null);
                return null;
            }
        }
    }
}