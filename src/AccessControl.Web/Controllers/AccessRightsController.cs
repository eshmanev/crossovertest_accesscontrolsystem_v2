﻿using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Models.AccessRights;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class AccessRightsController : Controller
    {
        private readonly IRequestClient<IAllowUserGroupAccess, IVoidResult> _allowUserGroupRequest;
        private readonly IRequestClient<IAllowUserAccess, IVoidResult> _allowUserRequest;
        private readonly IRequestClient<IDenyUserGroupAccess, IVoidResult> _denyUserGroupRequest;
        private readonly IRequestClient<IDenyUserAccess, IVoidResult> _denyUserRequest;
        private readonly IRequestClient<IListAccessPoints, IListAccessPointsResult> _listAccessPointsRequest;
        private readonly IRequestClient<IListAccessRights, IListAccessRightsResult> _listAccessRightsRequest;
        private readonly IRequestClient<IListUserGroups, IListUserGroupsResult> _listUserGroupsRequest;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;

        public AccessRightsController(IRequestClient<IListAccessRights, IListAccessRightsResult> listAccessRightsRequest,
                                      IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest,
                                      IRequestClient<IListUsers, IListUsersResult> listUsersRequest,
                                      IRequestClient<IListUserGroups, IListUserGroupsResult> listUserGroupsRequest,
                                      IRequestClient<IAllowUserAccess, IVoidResult> allowUserRequest,
                                      IRequestClient<IAllowUserGroupAccess, IVoidResult> allowUserGroupRequest,
                                      IRequestClient<IDenyUserAccess, IVoidResult> denyUserRequest,
                                      IRequestClient<IDenyUserGroupAccess, IVoidResult> denyUserGroupRequest)
        {
            Contract.Requires(listAccessRightsRequest != null);
            Contract.Requires(listAccessPointsRequest != null);
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(listUserGroupsRequest != null);
            Contract.Requires(allowUserRequest != null);
            Contract.Requires(allowUserGroupRequest != null);
            Contract.Requires(denyUserRequest != null);
            Contract.Requires(denyUserGroupRequest != null);

            _listAccessRightsRequest = listAccessRightsRequest;
            _listAccessPointsRequest = listAccessPointsRequest;
            _listUsersRequest = listUsersRequest;
            _listUserGroupsRequest = listUserGroupsRequest;
            _allowUserRequest = allowUserRequest;
            _allowUserGroupRequest = allowUserGroupRequest;
            _denyUserRequest = denyUserRequest;
            _denyUserGroupRequest = denyUserGroupRequest;
        }

        public async Task<ActionResult> Index()
        {
            var model = new AccessRightsIndexViewModel {Editor = new EditAccessRightsViewModel()};
            await Initialize(model);
            return View(model);
        }

        [HttpPost, ActionName("Index")]
        public async Task<ActionResult> Allow(AccessRightsIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await Initialize(model);
                return View(model);
            }

            var result = !string.IsNullOrWhiteSpace(model.Editor.UserName)
                             ? await _allowUserRequest.Request(new AllowDenyUserAccess(model.Editor.AccessPointId, model.Editor.UserName))
                             : await _allowUserGroupRequest.Request(new AllowDenyUserGroupAccess(model.Editor.AccessPointId, model.Editor.UserGroupName));

            if (!result.Succeded)
            {
                ModelState.AddModelError(string.Empty, result.Fault.Summary);
                await Initialize(model);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Deny(bool group, string userOrGroupName, Guid accessPointId)
        {
            var result = !group
                             ? await _denyUserRequest.Request(new AllowDenyUserAccess(accessPointId, userOrGroupName))
                             : await _denyUserGroupRequest.Request(new AllowDenyUserGroupAccess(accessPointId, userOrGroupName));

            if (!result.Succeded)
            {
                ModelState.AddModelError(string.Empty, result.Fault.Summary);
                return await Index();
            }

            return RedirectToAction("Index");
        }

        private async Task Initialize(AccessRightsIndexViewModel model)
        {
            var accessRightsTask = _listAccessRightsRequest.Request(ListCommand.Default);
            var usersTask = _listUsersRequest.Request(ListCommand.Default);
            var accessPointsTask = _listAccessPointsRequest.Request(ListCommand.Default);
            var userGroupsTask = _listUserGroupsRequest.Request(ListCommand.Default);

            await Task.WhenAll(accessRightsTask, usersTask, accessPointsTask, userGroupsTask);

            model.UserAccessRights = accessRightsTask.Result.UserAccessRights;
            model.UserGroupAccessRights = accessRightsTask.Result.UserGroupAccessRights;
            model.Editor.AccessPoints = accessPointsTask.Result.AccessPoints;
            model.Editor.Users = usersTask.Result.Users;
            model.Editor.UserGroups = userGroupsTask.Result.Groups;
        }
    }
}