using System;
using System.Diagnostics.Contracts;
using AccessControl.Contracts.Commands.Management;

namespace AccessControl.Contracts.CodeContracts
{
    /// <summary>
    ///     Represents a contract class for the <see cref="IDenyUserSchedule" /> interface.
    /// </summary>
    [ContractClassFor(typeof(IDenyUserSchedule))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IDenyUserScheduleContract : IDenyUserSchedule
    {
        public Guid AccessPointId => default(Guid);
        public string UserName => null;
    }
}