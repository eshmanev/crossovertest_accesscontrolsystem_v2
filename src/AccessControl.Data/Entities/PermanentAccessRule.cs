namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a permanent access rule.
    /// </summary>
    public class PermanentAccessRule : AccessRuleBase
    {
        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessRuleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}