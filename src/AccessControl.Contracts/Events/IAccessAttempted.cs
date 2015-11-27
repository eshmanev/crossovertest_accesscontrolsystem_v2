﻿using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Events
{
    /// <summary>
    ///     Occurs when a user attempts to access an access point.
    /// </summary>
    [ContractClass(typeof(IAccessAttemptedContract))]
    public interface IAccessAttempted
    {
        /// <summary>
        ///     Gets the access point identifier.
        /// </summary>
        /// <value>
        ///     The access point identifier.
        /// </value>
        Guid AccessPointId { get; }

        /// <summary>
        ///     Gets the date time created.
        /// </summary>
        /// <value>
        ///     The date time created.
        /// </value>
        DateTime DateTimeCreated { get; }

        /// <summary>
        ///     Gets a value indicating whether the attempt is failed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        bool Failed { get; }

        /// <summary>
        ///     Gets the name of the user.
        /// </summary>
        /// <value>
        ///     The name of the user.
        /// </value>
        string UserName { get; }
    }
}