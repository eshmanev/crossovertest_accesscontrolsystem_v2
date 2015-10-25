namespace AccessControl.LDAP.SDK
{
    /// <summary>
    /// Provides information about deparment manager.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class Manager : User
    {
        /// <summary>
        /// Gets or sets the manager's cell phone.
        /// </summary>
        public string CellPhone { get; set; }
    }
}