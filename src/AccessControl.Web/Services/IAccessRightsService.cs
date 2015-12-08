using System;
using System.Threading.Tasks;
using AccessControl.Contracts.Dto;

namespace AccessControl.Web.Services
{
    public interface IAccessRightsService
    {
        Task<IVoidResult> AllowUserAccess(string userName, Guid accessPointId);
        Task<IVoidResult> AllowGroupAccess(string groupName, Guid accessPointId);
        Task<IVoidResult> DenyUserAccess(string userName, Guid accessPointId);
        Task<IVoidResult> DenyGroupAccess(string groupName, Guid accessPointId);
        Task<IVoidResult> AllowScheduledUserAccess(string userName, Guid accessPointId, IWeeklySchedule weeklySchedule);
        Task<IVoidResult> AllowScheduledGroupAccess(string groupName, Guid accessPointId, IWeeklySchedule weeklySchedule);
        Task<IVoidResult> DenyScheduledUserAccess(string userName, Guid accessPointId);
        Task<IVoidResult> DenyScheduledGroupAccess(string groupName, Guid accessPointId);
    }
}