namespace AccessControl.Data.Entities
{
    /// <summary>
    /// Represents an entity containing information about delegated management rights.
    /// </summary>
    public class DelegatedRights
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the manager who granted his management rights.
        /// </summary>
        /// <value>
        /// The grantor.
        /// </value>
        public virtual string Grantor { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who is granted the management rights.
        /// </summary>
        /// <value>
        /// The grantee.
        /// </value>
        public virtual string Grantee { get; set; } 
    }
}