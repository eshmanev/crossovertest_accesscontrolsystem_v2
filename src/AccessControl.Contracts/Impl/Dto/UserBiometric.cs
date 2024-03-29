﻿using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.Impl.Dto
{
    public class UserBiometric : User, IUserBiometric
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserBiometric" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userGroups">The user groups.</param>
        public UserBiometric(string userName, IUserGroup[] userGroups)
            : base(userName, userGroups)
        {
        }

        /// <summary>
        ///     Gets or sets the biometric hash.
        /// </summary>
        /// <value>
        ///     The biometric hash.
        /// </value>
        public string BiometricHash { get; set; }
    }
}