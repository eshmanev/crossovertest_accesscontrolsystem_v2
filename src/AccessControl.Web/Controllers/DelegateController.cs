using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Web.Models.Delegate;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class DelegateController : Controller
    {
        private readonly IRequestClient<IGrantManagementRights, IVoidResult> _grantRequest;
        private readonly IRequestClient<IListDelegatedUsers, IListDelegatedUsersResult> _listDelegatedUsersRequest;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;
        private readonly IRequestClient<IRevokeManagementRights, IVoidResult> _revokeRequest;

        public DelegateController(IRequestClient<IListUsers, IListUsersResult> listUsersRequest,
                                  IRequestClient<IListDelegatedUsers, IListDelegatedUsersResult> listDelegatedUsersRequest,
                                  IRequestClient<IGrantManagementRights, IVoidResult> grantRequest,
                                  IRequestClient<IRevokeManagementRights, IVoidResult> revokeRequest)
        {
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(listDelegatedUsersRequest != null);
            Contract.Requires(grantRequest != null);
            Contract.Requires(revokeRequest != null);

            _listUsersRequest = listUsersRequest;
            _listDelegatedUsersRequest = listDelegatedUsersRequest;
            _grantRequest = grantRequest;
            _revokeRequest = revokeRequest;
        }

        [HttpPost]
        public async Task<JsonResult> Grant(string userName)
        {
            var result = await _grantRequest.Request(new GrantRevokeManagementRights(userName));
            return !result.Succeded
                       ? Json(new {Succeded = false, result.Fault}, JsonRequestBehavior.AllowGet)
                       : Json(new {Succeded = true}, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Index()
        {
            var listUsersTask = _listUsersRequest.Request(ListCommand.WithoutParameters);
            var listDelegatedTask = _listDelegatedUsersRequest.Request(ListCommand.WithoutParameters);

            await Task.WhenAll(listUsersTask, listDelegatedTask);

            var delegated = new HashSet<string>(listDelegatedTask.Result.UserNames);
            var model = new IndexViewModel
            {
                Users = listUsersTask.Result.Users
                                     .Select(x => new UserViewModel {User = x, IsDelegated = delegated.Contains(x.UserName)})
                                     .ToArray()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Revoke(string userName)
        {
            var result = await _revokeRequest.Request(new GrantRevokeManagementRights(userName));
            return !result.Succeded
                       ? Json(new {Succeded = false, result.Fault}, JsonRequestBehavior.AllowGet)
                       : Json(new {Succeded = true}, JsonRequestBehavior.AllowGet);
        }
    }
}