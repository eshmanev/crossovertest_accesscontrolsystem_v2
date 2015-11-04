namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a base class for access rights.
    /// </summary>
    public class AccessRightsBase
    {
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public virtual int Id { get; protected set; }

        public virtual void AddAccessRule(AccessRuleBase accessRule)
        {
            
        }
    }
}