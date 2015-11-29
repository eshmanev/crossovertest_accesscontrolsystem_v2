using System;
using System.Collections;
using System.Collections.Generic;

namespace AccessControl.Data.Entities
{
    /// <summary>
    /// Represents an access point entity.
    /// </summary>
    public class AccessPoint
    {
        /// <summary>
        /// Gets or sets the access point identifier.
        /// </summary>
        /// <value>
        /// The access point identifier.
        /// </value>
        public virtual Guid AccessPointId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public virtual string Site { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        /// <value>
        /// The department.
        /// </value>
        public virtual string Department { get; set; }

        /// <summary>
        /// Gets or sets the managed by.
        /// </summary>
        /// <value>
        /// The managed by.
        /// </value>
        public virtual string ManagedBy { get; set; }

        // NHibernate workaround. Cascade delete does not work without this properties.
        protected internal virtual IList<AccessRuleBase> AccessRules { get; set; }
        protected internal virtual IList<LogEntry> Logs { get; set; }
    }
}