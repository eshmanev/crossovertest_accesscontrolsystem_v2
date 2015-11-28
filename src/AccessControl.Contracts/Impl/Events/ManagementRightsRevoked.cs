using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class ManagementRightsRevoked : IManagementRightsRevoked
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ManagementRightsRevoked" /> class.
        /// </summary>
        /// <param name="grantor">The grantor.</param>
        /// <param name="grantee">The grantee.</param>
        public ManagementRightsRevoked(string grantor, string grantee)
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