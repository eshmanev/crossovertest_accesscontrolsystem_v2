using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Models.AccessRights;
using AccessControl.Web.Services;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class AccessRightsController : Controller
    {
        private readonly IRequestClient<IListAccessRights, IListAccessRightsResult> _listAccessRightsRequest;
        private readonly IRequestClient<IListAccessPoints, IListAccessPointsResult> _listAccessPointsRequest;
        private readonly IRequestClient<IFindUsersByDepartment, IFindUsersByDepartmentResult> _listUsersRequest;
        private readonly IRequestClient<IListUserGroups, IListUserGroupsResult> _listUserGroupsRequest;
        private readonly IRequestClient<IAllowUserAccess, IVoidResult> _allowUserRequest;
        private readonly IRequestClient<IAllowUserGroupAccess, IVoidResult> _allowUserGroupRequest;
        private readonly IRequestClient<IDenyUserAccess, IVoidResult> _denyUserRequest;
        private readonly IRequestClient<IDenyUserGroupAccess, IVoidResult> _denyUserGroupRequest;

        public AccessRightsController(IRequestClient<IListAccessRights, IListAccessRightsResult> listAccessRightsRequest,
                                      IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest,
                                      IRequestClient<IFindUsersByDepartment, IFindUsersByDepartmentResult> listUsersRequest,
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

        [HttpPost]
        public async Task<ActionResult> Index(AccessRightsIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await Initialize(model);
                return View(model);
            }

            IVoidResult result = !string.IsNullOrWhiteSpace(model.Editor.UserName)
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

        private async Task Initialize(AccessRightsIndexViewModel model)
        {
            var user = HttpContext.GetApplicationUser();
            var usersTask = _listUsersRequest.Request(new FindUsersByDepartment(user.Site, user.Department));
            var accessPointsTask = _listAccessPointsRequest.Request(new ListAccessPoints(user.Site, user.Department));
            var userGroupsTask = _listUserGroupsRequest.Request(new ListUserGroups(user.Site, user.Department));

            await Task.WhenAll(usersTask, accessPointsTask, userGroupsTask);

            model.Editor.AccessPoints = accessPointsTask.Result.AccessPoints;
            model.Editor.Users = usersTask.Result.Users;
            model.Editor.UserGroups = userGroupsTask.Result.Groups;
        }
    }
}