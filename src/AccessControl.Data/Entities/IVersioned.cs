using System;

namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Defines a versioned entity.
    /// </summary>
    public interface IVersioned
    {
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        Guid Id { get; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        ulong Version { get; set; }
    }
}