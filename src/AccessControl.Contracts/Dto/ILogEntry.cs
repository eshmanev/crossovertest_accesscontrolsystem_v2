using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Dto
{
    /// <summary>
    /// </summary>
    [ContractClass(typeof(ILogEntryContract))]
    public interface ILogEntry
    {
        /// <summary>
        ///     Gets the attempted access point.
        /// </summary>
        /// <value>
        ///     The attempted access point.
        /// </value>
        IAccessPoint AttemptedAccessPoint { get; }

        /// <summary>
        ///     Gets the attempted hash.
        /// </summary>
        /// <value>
        ///     The attempted hash.
        /// </value>
        string AttemptedHash { get; }

        /// <summary>
        ///     Gets the date created, UTC.
        /// </summary>
        /// <value>
        ///     The date created, UTC.
        /// </value>
        DateTime CreatedUtc { get; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="ILogEntry" /> is failed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        bool Failed { get; }

        /// <summary>
        ///     Gets the user. If the attempted hash is unknown, when the property returns null.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        IUser User { get; }
    }
}