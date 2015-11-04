namespace AccessControl.Data.Entities
{
    /// <summary>
    /// Represents a user-specific access rights.
    /// </summary>
    public class UserAccessRights : AccessRightsBase
    {
        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public virtual string UserName { get; set; }
    }
}