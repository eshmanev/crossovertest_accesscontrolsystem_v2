using System.Collections.Generic;

namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a scheduled access rule.
    /// </summary>
    public class ScheduledAccessRule : AccessRuleBase
    {
        private IList<SchedulerEntry> _entries = new List<SchedulerEntry>();

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        public virtual string TimeZone { get; set; }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <value>
        /// The entries.
        /// </value>
        public virtual IEnumerable<SchedulerEntry> Entries
        {
            get { return _entries; }
            protected set { _entries = (IList<SchedulerEntry>)value; }
        }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessRuleVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        ///     Adds the scheduler entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public virtual void AddEntry(SchedulerEntry entry)
        {
            if (!_entries.Contains(entry))
            {
                _entries.Add(entry);
                entry.Rule = this;
            }
        }

        /// <summary>
        ///     Removes the scheduler entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public virtual void RemoveEntry(SchedulerEntry entry)
        {
            if (_entries.Contains(entry))
            {
                _entries.Remove(entry);
                entry.Rule = null;
            }
        }
    }
}