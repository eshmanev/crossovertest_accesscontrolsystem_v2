using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands;
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

        public AccessRightsController(IRequestClient<IListAccessRights, IListAccessRightsResult> listAccessRightsRequest,
            IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest,
            IRequestClient<IFindUsersByDepartment, IFindUsersByDepartmentResult> listUsersRequest,
            IRequestClient<IListUserGroups, IListUserGroupsResult> listUserGroupsRequest)
        {
            Contract.Requires(listAccessRightsRequest != null);
            Contract.Requires(listAccessPointsRequest != null);
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(listUserGroupsRequest != null
                );

            _listAccessRightsRequest = listAccessRightsRequest;
            _listAccessPointsRequest = listAccessPointsRequest;
            _listUsersRequest = listUsersRequest;
            _listUserGroupsRequest = listUserGroupsRequest;
        }

        public async Task<ActionResult> Index()
        {
            var model = new AccessRightsIndexViewModel {Editor = new EditAccessRightsViewModel()};
            await Initialize(model);
            return View(model);
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