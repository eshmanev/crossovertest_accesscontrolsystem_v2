using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands;

namespace AccessControl.Contracts.Helpers
{
    public class ListUserGroups : IListUserGroups
    {
        public ListUserGroups(string site, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Department = department;
            Site = site;
        }

        public string Department { get; }
        public string Site { get; }
    }
}