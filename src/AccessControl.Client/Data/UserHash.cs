using System;
using System.Diagnostics.Contracts;

namespace AccessControl.Client.Data
{
    /// <summary>
    ///     Contains user hash.
    /// </summary>
    [Serializable]
    internal struct UserHash
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserHash" /> struct.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="hash">The hash.</param>
        public UserHash(string userName, string hash)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            UserName = userName;
            Hash = hash;
        }

        /// <summary>
        ///     Gets the hash.
        /// </summary>
        /// <value>
        ///     The hash.
        /// </value>
        public string Hash { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }
    }
}