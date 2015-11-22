using System;

namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a scheduled access rule.
    /// </summary>
    public class ScheduledAccessRule : AccessRuleBase
    {
        /// <summary>
        ///     Gets or sets from time in UTC format.
        /// </summary>
        /// <value>
        ///     From time UTC.
        /// </value>
        public virtual TimeSpan FromTimeUtc { get; set; }

        /// <summary>
        ///     Gets or sets to time in UTC format.
        /// </summary>
        /// <value>
        ///     To time UTC.
        /// </value>
        public virtual TimeSpan ToTimeUtc { get; set; }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessRuleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}