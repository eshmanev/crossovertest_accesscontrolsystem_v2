using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using MassTransit;

namespace AccessControl.Web.Services
{
    internal class AccessRightsService : IAccessRightsService
    {
        private readonly IBus _bus;

        public AccessRightsService(IBus bus)
        {
            Contract.Requires(bus != null);
            _bus = bus;
        }

        public Task<IVoidResult> AllowUserAccess(string userName, Guid accessPointId)
        {
            return _bus.AccessControlClient<IAllowUserAccess, IVoidResult>().Request(new AllowUserAccess(accessPointId, userName));
        }

        public Task<IVoidResult> AllowGroupAccess(string groupName, Guid accessPointId)
        {
            return _bus.AccessControlClient<IAllowUserGroupAccess, IVoidResult>().Request(new AllowUserGroupAccess(accessPointId, groupName));
        }

        public Task<IVoidResult> DenyUserAccess(string userName, Guid accessPointId)
        {
            return _bus.AccessControlClient<IDenyUserAccess, IVoidResult>().Request(new DenyUserAccess(accessPointId, userName));
        }

        public Task<IVoidResult> DenyGroupAccess(string groupName, Guid accessPointId)
        {
            return _bus.AccessControlClient<IDenyUserGroupAccess, IVoidResult>().Request(new DenyUserGroupAccess(accessPointId, groupName));
        }

        public Task<IVoidResult> AllowScheduledUserAccess(string userName, Guid accessPointId, IWeeklySchedule weeklySchedule)
        {
            return _bus.AccessControlClient<IScheduleUserAccess, IVoidResult>().Request(new ScheduleUserAccess(accessPointId, userName, weeklySchedule));
        }

        public Task<IVoidResult> AllowScheduledGroupAccess(string groupName, Guid accessPointId, IWeeklySchedule weeklySchedule)
        {
            return _bus.AccessControlClient<IScheduleUserGroupAccess, IVoidResult>().Request(new ScheduleUserGroupAccess(accessPointId, groupName, weeklySchedule));
        }

        public Task<IVoidResult> DenyScheduledUserAccess(string userName, Guid accessPointId)
        {
            return _bus.AccessControlClient<IRemoveUserSchedule, IVoidResult>().Request(new RemoveUserSchedule(accessPointId, userName));
        }

        public Task<IVoidResult> DenyScheduledGroupAccess(string groupName, Guid accessPointId)
        {
            return _bus.AccessControlClient<IRemoveGroupSchedule, IVoidResult>().Request(new RemoveGroupSchedule(accessPointId, groupName));
        }
    }
}