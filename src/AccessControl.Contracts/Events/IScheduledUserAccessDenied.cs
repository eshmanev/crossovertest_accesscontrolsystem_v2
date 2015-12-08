using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    [ContractClass(typeof(IScheduledUserAccessDeniedContract))]
    public interface IScheduledUserAccessDenied
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}