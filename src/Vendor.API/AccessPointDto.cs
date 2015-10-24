using System;
using System.Runtime.Serialization;

namespace Vendor.API
{
    [DataContract(Name = "AccessPoint")]
    public class AccessPointDto
    {
        [DataMember(IsRequired = true)]
        public Guid AccessPointId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }
    }
}