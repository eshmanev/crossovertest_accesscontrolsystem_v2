namespace AccessControl.Data.Entities
{
    /// <summary>
    /// Represents a visitor of access rights.
    /// </summary>
    public interface IAccessRightsVisitor
    {
        /// <summary>
        /// Visits the specified user access rights.
        /// </summary>
        /// <param name="accessRights">The access rights.</param>
        void Visit(UserAccessRights accessRights);

        /// <summary>
        /// Visits the specified user group access rights.
        /// </summary>
        /// <param name="accessRights">The access rights.</param>
        void Visit(UserGroupAccessRights accessRights);
    }
}