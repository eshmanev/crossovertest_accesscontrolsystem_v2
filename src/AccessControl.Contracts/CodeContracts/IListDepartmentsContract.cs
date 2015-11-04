

namespace AccessControl.Contracts.CodeContracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts.Commands;

    /// <summary>
    /// Represents a contract class for the <see cref="IListDepartments" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IListDepartments))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IListDepartmentsContract : IListDepartments
    {
         
    }
}