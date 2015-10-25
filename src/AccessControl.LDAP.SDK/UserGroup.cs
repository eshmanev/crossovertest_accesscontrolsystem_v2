namespace AccessControl.LDAP.SDK
{
    /// <summary>
    /// Provides information about user group.
    /// </summary>
    // ReSharper disable once InconsistentNaming    
    public class UserGroup
    {
        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}