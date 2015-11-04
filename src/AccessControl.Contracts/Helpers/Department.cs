using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class Department : IDepartment
    {
        public Department(string siteDistinguishedName, string site, string departmentName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(siteDistinguishedName));
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(departmentName));

            Site = site;
            DepartmentName = departmentName;
            SiteDistinguishedName = siteDistinguishedName;
        }

        public string Site { get; }

        public string SiteDistinguishedName { get; }

        public string DepartmentName { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Department;
            if (other == null)
                return false;

            return other.SiteDistinguishedName == SiteDistinguishedName &&
                   other.Site == Site &&
                   other.DepartmentName == DepartmentName;
        }

        public override int GetHashCode()
        {
            return SiteDistinguishedName.GetHashCode() ^ Site.GetHashCode() ^ DepartmentName.GetHashCode();
        }
    }
}