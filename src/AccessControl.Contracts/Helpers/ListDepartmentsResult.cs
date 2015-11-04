using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class ListDepartmentsResult : IListDepartmentsResult
    {
        public ListDepartmentsResult(IDepartment[] departments)
        {
            Contract.Requires(departments != null);
            Departments = departments;
        }

        public IDepartment[] Departments { get; }
    }
}