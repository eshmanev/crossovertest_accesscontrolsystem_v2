namespace AccessControl.Data.Entities
{
    /// <summary>
    /// Represents an abstract base class for access rules.
    /// </summary>
    public abstract class AccessRuleBase
    {
        /// <summary>
        ///     Gets or sets the access point.
        /// </summary>
        public virtual AccessPoint AccessPoint { get; set; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Gets the referenced access rights.
        /// </summary>
        public virtual AccessRightsBase AccessRights { get; protected internal set; }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public abstract void Accept(IAccessRuleVisitor visitor);
    }
}