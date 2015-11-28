using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Search;

namespace AccessControl.Contracts.Impl.Commands
{
    public class FindAccessPointById : IFindAccessPointById
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindAccessPointById" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        public FindAccessPointById(Guid accessPointId)
        {
            Contract.Requires(accessPointId != Guid.Empty);
            AccessPointId = accessPointId;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }
    }
}