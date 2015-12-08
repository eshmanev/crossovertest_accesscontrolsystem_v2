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
        Task<IVoidResult> ScheduleUserAccess(string userName, Guid accessPointId, ISchedule schedule);
        Task<IVoidResult> ScheduleGroupAccess(string groupName, Guid accessPointId, ISchedule schedule);
    }
}