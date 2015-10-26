using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.Helpers
{
    public class FindUsersByDepartment : IFindUsersByDepartment
    {
        public FindUsersByDepartment(string site, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Department = department;
            Site = site;
        }

        public string Site { get; }
        public string Department { get; }
    }
}