namespace AccessControl.Data.Entities
{
    /// <summary>
    /// Represents a visitor of access rules.
    /// </summary>
    public interface IAccessRuleVisitor
    {
        /// <summary>
        /// Visits the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        void Visit(PermanentAccessRule rule);

        /// <summary>
        /// Visits the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        void Visit(ScheduledAccessRule rule);
    }
}