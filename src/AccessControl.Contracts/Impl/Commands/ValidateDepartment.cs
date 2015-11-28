using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.Impl.Commands
{
    public class ValidateDepartment : IValidateDepartment
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateDepartment" /> class.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="department">The department.</param>
        public ValidateDepartment(string site, string department)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(site));
            Contract.Requires(!string.IsNullOrWhiteSpace(department));
            Site = site;
            Department = department;
        }

        /// <summary>
        ///     Gets the department.
        /// </summary>
        /// <value>
        ///     The department.
        /// </value>
        public string Department { get; }

        /// <summary>
        ///     Gets the site.
        /// </summary>
        /// <value>
        ///     The site.
        /// </value>
        public string Site { get; }
    }
}