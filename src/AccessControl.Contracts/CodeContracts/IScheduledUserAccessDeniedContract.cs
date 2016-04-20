﻿using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IScheduledUserAccessDenied" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IScheduledUserAccessDenied))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IScheduledUserAccessDeniedContract : IScheduledUserAccessDenied
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }
    }
}