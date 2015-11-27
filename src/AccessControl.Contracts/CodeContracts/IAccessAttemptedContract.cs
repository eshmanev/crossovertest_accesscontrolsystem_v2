using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Events;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IAccessAttempted" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IAccessAttempted))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAccessAttemptedContract : IAccessAttempted
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return Guid.Empty;
            }
        }

        public DateTime DateTimeCreated
        {
            get
            {
                Contract.Ensures(Contract.Result<DateTime>() != DateTime.MinValue);
                return default(DateTime);
            }
        }

        public bool Failed => false;

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