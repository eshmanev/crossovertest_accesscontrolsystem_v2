namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a user group-specific access rights.
    /// </summary>
    public class UserGroupAccessRights : AccessRightsBase
    {
        /// <summary>
        ///     Gets or sets the user's group name.
        /// </summary>
        public virtual string UserGroupName { get; set; }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IAccessRightsVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}