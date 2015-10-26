using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Helpers
{
    public class AccessPoint : IAccessPoint
    {
        public AccessPoint(Guid accessPointId, string site, string department, string name)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            AccessPointId = accessPointId;
            Site = site;
            Department = department;
            Name = name;
        }

        public Guid AccessPointId { get; }
        public string Department { get; }
        public string Site { get; }
        public string Name { get; }
        public string Description { get; set; }
    }
}