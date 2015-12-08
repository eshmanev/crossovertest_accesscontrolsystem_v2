using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Web.Models.AccessRights;
using AccessControl.Web.Services;
using MassTransit;
using TimeRange = AccessControl.Web.Models.AccessRights.TimeRange;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class AccessRightsController : Controller
    {
        private readonly IRequestClient<IListAccessPoints, IListAccessPointsResult> _listAccessPointsRequest;
        private readonly IRequestClient<IListAccessRights, IListAccessRightsResult> _listAccessRightsRequest;
        private readonly IRequestClient<IListUserGroups, IListUserGroupsResult> _listUserGroupsRequest;
        private readonly IAccessRightsService _accessRightsService;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;

        public AccessRightsController(IRequestClient<IListAccessRights, IListAccessRightsResult> listAccessRightsRequest,
                                      IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest,
                                      IRequestClient<IListUsers, IListUsersResult> listUsersRequest,
                                      IRequestClient<IListUserGroups, IListUserGroupsResult> listUserGroupsRequest,
                                      IAccessRightsService accessRightsService)
        {
            Contract.Requires(listAccessRightsRequest != null);
            Contract.Requires(listAccessPointsRequest != null);
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(listUserGroupsRequest != null);
            Contract.Requires(accessRightsService != null);

            _listAccessRightsRequest = listAccessRightsRequest;
            _listAccessPointsRequest = listAccessPointsRequest;
            _listUsersRequest = listUsersRequest;
            _listUserGroupsRequest = listUserGroupsRequest;
            _accessRightsService = accessRightsService;
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> Allow(AccessRightsIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await Initialize(model);
                return View(model);
            }

            IVoidResult result = null;
            if (model.Editor.ScheduleApplied)
            {
                if (TimeZoneInfo.FindSystemTimeZoneById(model.Editor.SchedulerTimeZone) == null)
                {
                    ModelState.AddModelError("Editor_SchedulerTimeZone", "Invalid time zone");
                }

                if (ModelState.IsValid)
                {
                    result = await ScheduleAccess(model.Editor);
                }
            }
            else
            {
                result = await AllowPermanentAccess(model.Editor);
            }

            if (result != null && !result.Succeded)
            {
                ModelState.AddModelError(string.Empty, result.Fault.Summary);
            }

            if (!ModelState.IsValid)
            {
                await Initialize(model);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DenyPermanent(bool group, string userOrGroupName, Guid accessPointId)
        {
            var result = !group
                             ? await _accessRightsService.DenyUserAccess(userOrGroupName, accessPointId)
                             : await _accessRightsService.DenyGroupAccess(userOrGroupName, accessPointId);

            if (!result.Succeded)
            {
                ModelState.AddModelError(string.Empty, result.Fault.Summary);
                return await Index();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DenySchedule(bool group, string userOrGroupName, Guid accessPointId)
        {
            var result = !group
                             ? await _accessRightsService.DenyScheduledUserAccess(userOrGroupName, accessPointId)
                             : await _accessRightsService.DenyScheduledGroupAccess(userOrGroupName, accessPointId);

            if (!result.Succeded)
            {
                ModelState.AddModelError(string.Empty, result.Fault.Summary);
                return await Index();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index()
        {
            var model = new AccessRightsIndexViewModel {Editor = new EditAccessRightsViewModel()};
            await Initialize(model);
            return View(model);
        }

        private Task<IVoidResult> AllowPermanentAccess(EditAccessRightsViewModel editor)
        {
            return string.IsNullOrWhiteSpace(editor.UserName)
                       ? _accessRightsService.AllowGroupAccess(editor.UserGroupName, editor.AccessPointId)
                       : _accessRightsService.AllowUserAccess(editor.UserName, editor.AccessPointId);
        }

        private Task<IVoidResult> ScheduleAccess(EditAccessRightsViewModel editor)
        {
            var schedule = new WeeklySchedule(editor.SchedulerTimeZone);
            foreach (var item in editor.TimeRangePerDays)
            {
                DayOfWeek day;
                if (!Enum.TryParse(item.Key, out day))
                {
                    throw new HttpRequestValidationException();
                }

                schedule.DailyTimeRange.Add(day, new Contracts.Impl.Dto.TimeRange(item.Value.FromTime, item.Value.ToTime));
            }

            return string.IsNullOrWhiteSpace(editor.UserName)
                       ? _accessRightsService.AllowScheduledGroupAccess(editor.UserGroupName, editor.AccessPointId, schedule)
                       : _accessRightsService.AllowScheduledUserAccess(editor.UserName, editor.AccessPointId, schedule);
        }

        private async Task Initialize(AccessRightsIndexViewModel model)
        {
            var accessRightsTask = _listAccessRightsRequest.Request(ListCommand.WithoutParameters);
            var usersTask = _listUsersRequest.Request(ListCommand.WithoutParameters);
            var accessPointsTask = _listAccessPointsRequest.Request(ListCommand.WithoutParameters);
            var userGroupsTask = _listUserGroupsRequest.Request(ListCommand.WithoutParameters);

            await Task.WhenAll(accessRightsTask, usersTask, accessPointsTask, userGroupsTask);

            model.UserAccessRights = accessRightsTask.Result.UserAccessRights;
            model.UserGroupAccessRights = accessRightsTask.Result.UserGroupAccessRights;
            model.Editor.AccessPoints = accessPointsTask.Result.AccessPoints;
            model.Editor.Users = usersTask.Result.Users;
            model.Editor.UserGroups = userGroupsTask.Result.Groups;
            model.Editor.SchedulerTimeZone = TimeZoneInfo.Local.Id;
            model.Editor.TimeRangePerDays = new Dictionary<string, TimeRange>();
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                model.Editor.TimeRangePerDays[day.ToString()] = new TimeRange {FromTime = new TimeSpan(9, 0, 0), ToTime = new TimeSpan(18, 0, 0)};
            }
        }
    }
}