using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Dto;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="ILogEntry" /> interface.
    /// </summary>
    [ContractClassFor(typeof(ILogEntry))]
    // ReSharper disable once InconsistentNaming
    internal abstract class ILogEntryContract : ILogEntry
    {
        public IAccessPoint AttemptedAccessPoint
        {
            get
            {
                Contract.Ensures(Contract.Result<IAccessPoint>() != null);
                return null;
            }
        }

        public string AttemptedHash
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                return null;
            }
        }

        public DateTime CreatedUtc => DateTime.MinValue;

        public bool Failed => false;

        public IUser User => null;
    }
}