using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.Helpers
{
    public class ValidateDepartment : IValidateDepartment
    {
        public ValidateDepartment(string site, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Site = site;
            Department = department;
        }

        public string Department { get; }
        public string Site { get; }
    }
}