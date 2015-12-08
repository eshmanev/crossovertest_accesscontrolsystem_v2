

using System;

namespace AccessControl.Contracts.CodeContracts
{
    using System.Diagnostics.Contracts;
    using AccessControl.Contracts.Commands.Management;

    /// <summary>
    /// Represents a contract class for the <see cref="IRemoveUserSchedule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IRemoveUserSchedule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IRemoveUserScheduleContract : IRemoveUserSchedule
    {
        public Guid AccessPointId
        {
            get
            {
                Contract.Ensures(Contract.Result<Guid>() != Guid.Empty);
                return default(Guid);
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