using System.Diagnostics.Contracts;
using AccessControl.Contracts.CodeContracts;

namespace AccessControl.Contracts.Commands.Management
{
    /// <summary>
    ///     Removes the schedule for the specified user.
    /// </summary>
    [ContractClass(typeof(IDenyUserScheduleContract))]
    public interface IDenyUserSchedule : IDenyUserAccess
    {
    }
}