using System;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.Impl.Events
{
    public class PermanentUserAccessAllowed : IPermanentUserAccessAllowed
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PermanentUserAccessAllowed" /> class.
        /// </summary>
        /// <param name="accessPointId">The access point identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="biometricHash">The biometric hash.</param>
        public PermanentUserAccessAllowed(Guid accessPointId, string userName, string biometricHash)
        {
            AccessPointId = accessPointId;
            UserName = userName;
            BiometricHash = biometricHash;
        }

        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        public Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the user's biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        public string BiometricHash { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }
    }
}