using AccessControl.LDAP.SDK;

namespace AccessControl.LDAP.SDK
{
    /// <summary>
    /// Provides information of user department.
    /// </summary>
    public class UserDepartment
    {
        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public Site Site { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        public Department Department { get; set; }
    }
}