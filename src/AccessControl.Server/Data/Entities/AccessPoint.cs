using System;

namespace AccessControl.Server.Data.Entities
{
    public class AccessPoint
    {
        public virtual Guid AccessPointId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}