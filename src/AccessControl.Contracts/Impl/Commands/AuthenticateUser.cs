﻿using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Security;

namespace AccessControl.Contracts.Impl.Commands
{
    public class AuthenticateUser : IAuthenticateUser
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthenticateUser" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public AuthenticateUser(string userName, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            UserName = userName;
            Password = password;
        }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        public string UserName { get; }

        /// <summary>
        ///     Gets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        public string Password { get; }
    }
}