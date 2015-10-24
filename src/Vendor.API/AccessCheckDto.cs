using System;
using System.Runtime.Serialization;

namespace Vendor.API
{
    [DataContract(Name = "CheckAccess")]
    public class AccessCheckDto
    {
        /// <summary>
        /// The access point's unique identifier.
        /// </summary>
        [DataMember(IsRequired = true)]
        public Guid AccessPointId { get; set; }

        /// <summary>
        /// A hash of the user to check access rights.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UserHash { get; set; }
    }
}