using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Contracts.Impl
{
    public class RegisterAccessPoint : IRegisterAccessPoint
    {
        public RegisterAccessPoint(Guid accessPointId, string name, string description)
        {
            Contract.Requires(accessPointId != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            AccessPointId = accessPointId;
            Description = description;
            Name = name;
        }

        public Guid AccessPointId { get; }
        public string Description { get; }
        public string Name { get; }
    }
}