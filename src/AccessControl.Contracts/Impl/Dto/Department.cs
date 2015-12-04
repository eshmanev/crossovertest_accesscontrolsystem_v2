using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class Department : IDepartment
    {
        public Department(string departmentName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(departmentName));
            DepartmentName = departmentName;
        }

        public string DepartmentName { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Department;
            if (other == null)
                return false;

            return other.DepartmentName == DepartmentName;
        }

        public override int GetHashCode()
        {
            return DepartmentName.GetHashCode();
        }
    }
}