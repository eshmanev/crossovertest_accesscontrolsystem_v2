using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class AccessPoint : IAccessPoint
    {
        public AccessPoint(Guid accessPointId, string department, string name)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            AccessPointId = accessPointId;
            Department = department;
            Name = name;
        }

        public Guid AccessPointId { get; }
        public string Department { get; }
        public string Name { get; }
        public string Description { get; set; }
    }
}