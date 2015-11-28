using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class ManagementRightsGranted : IManagementRightsGranted
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ManagementRightsGranted" /> class.
        /// </summary>
        /// <param name="grantor">The grantor.</param>
        /// <param name="grantee">The grantee.</param>
        public ManagementRightsGranted(string grantor, string grantee)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(grantor));
            Contract.Requires(!string.IsNullOrWhiteSpace(grantee));
            Grantor = grantor;
            Grantee = grantee;
        }

        /// <summary>
        ///     Gets the grantee.
        /// </summary>
        /// <value>
        ///     The grantee.
        /// </value>
        public string Grantee { get; }

        /// <summary>
        ///     Gets the grantor.
        /// </summary>
        /// <value>
        ///     The grantor.
        /// </value>
        public string Grantor { get; }
    }
}