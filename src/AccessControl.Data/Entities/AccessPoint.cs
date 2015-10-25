using System;

namespace AccessControl.Data.Entities
{
    public class AccessPoint
    {
        public virtual Guid AccessPointId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}