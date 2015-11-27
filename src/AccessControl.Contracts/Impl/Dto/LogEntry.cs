using System;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class LogEntry : ILogEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LogEntry" /> class.
        /// </summary>
        /// <param name="createdUtc">The created UTC.</param>
        /// <param name="attemptedAccessPoint">The attempted access point.</param>
        /// <param name="attemptedHash">The attempted hash.</param>
        /// <param name="user">The user.</param>
        /// <param name="failed">if set to <c>true</c> [failed].</param>
        public LogEntry(DateTime createdUtc, IAccessPoint attemptedAccessPoint, string attemptedHash, IUser user, bool failed)
        {
            CreatedUtc = createdUtc;
            AttemptedAccessPoint = attemptedAccessPoint;
            AttemptedHash = attemptedHash;
            User = user;
            Failed = failed;
        }

        /// <summary>
        ///     Gets the attempted access point.
        /// </summary>
        /// <value>
        ///     The attempted access point.
        /// </value>
        public IAccessPoint AttemptedAccessPoint { get; }

        /// <summary>
        ///     Gets the attempted hash.
        /// </summary>
        /// <value>
        ///     The attempted hash.
        /// </value>
        public string AttemptedHash { get; }

        /// <summary>
        ///     Gets the date created, UTC.
        /// </summary>
        /// <value>
        ///     The date created, UTC.
        /// </value>
        public DateTime CreatedUtc { get; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="ILogEntry" /> is failed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed { get; }

        /// <summary>
        ///     Gets the user. If the attempted hash is unknown, when the property returns null.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public IUser User { get; }
    }
}