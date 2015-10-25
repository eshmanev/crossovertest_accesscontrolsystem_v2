namespace AccessControl.Server.Data.Entities
{
    /// <summary>
    /// Defines a map between user name and user physical hash.
    /// </summary>
    public class UserHashMap
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public virtual int Id { get; protected set; }
        
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user hash.
        /// </summary>
        public virtual string UserHash { get; set; }
    }
}