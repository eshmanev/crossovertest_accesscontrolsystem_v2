using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class Department : IDepartment
    {
        public Department(string site, string siteName, string departmentName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(siteName));
            Contract.Requires(!string.IsNullOrWhiteSpace(departmentName));

            SiteName = siteName;
            DepartmentName = departmentName;
            Site = site;
        }

        public string SiteName { get; }

        public string Site { get; }

        public string DepartmentName { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Department;
            if (other == null)
                return false;

            return other.Site == Site &&
                   other.SiteName == SiteName &&
                   other.DepartmentName == DepartmentName;
        }

        public override int GetHashCode()
        {
            return Site.GetHashCode() ^ SiteName.GetHashCode() ^ DepartmentName.GetHashCode();
        }
    }
}