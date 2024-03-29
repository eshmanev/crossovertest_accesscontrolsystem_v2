﻿using System;

namespace AccessControl.Data.Entities
{
    public class LogEntry
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the attempted access point.
        /// </summary>
        /// <value>
        /// The access point.
        /// </value>
        public virtual AccessPoint AccessPoint { get; set; }

        /// <summary>
        /// Gets or sets the name of the user. This property can be null in case of unknown attampted hash.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Gets or sets the attempted hash.
        /// </summary>
        /// <value>
        /// The attempted hash.
        /// </value>
        public virtual string AttemptedHash { get; set; }

        /// <summary>
        /// Gets or sets the date created, UTC.
        /// </summary>
        /// <value>
        /// The date created, UTC.
        /// </value>
        public virtual DateTime CreatedUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attempt has been failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the attempt has been failed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Failed { get; set; }
    }
}