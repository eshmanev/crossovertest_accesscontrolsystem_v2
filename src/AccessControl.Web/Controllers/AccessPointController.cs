using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Models;
using AccessControl.Web.Models.AccessPoint;
using AccessControl.Web.Services;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class AccessPointController : Controller
    {
        private readonly IRequestClient<IListAccessPoints, IListAccessPointsResult> _listAccessPointsRequest;
        private readonly IRequestClient<IListDepartments, IListDepartmentsResult> _listDepartmentsRequest;
        private readonly IRequestClient<IRegisterAccessPoint, IVoidResult> _registerAccessPointRequest;

        public AccessPointController(IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest,
                                     IRequestClient<IListDepartments, IListDepartmentsResult> listDepartmentsRequest,
                                     IRequestClient<IRegisterAccessPoint, IVoidResult> registerAccessPointRequest)
        {
            Contract.Requires(listAccessPointsRequest != null);
            Contract.Requires(listDepartmentsRequest != null);
            Contract.Requires(_registerAccessPointRequest != null);

            _listAccessPointsRequest = listAccessPointsRequest;
            _listDepartmentsRequest = listDepartmentsRequest;
            _registerAccessPointRequest = registerAccessPointRequest;
        }

        public async Task<ActionResult> Index()
        {
            var model = new AccessPointIndexViewModel {Editor = new EditAccessPointViewModel()};
            await Initialize(model);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(AccessPointIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await Initialize(model);
                return View(model);
            }

            var accessPoint = new AccessPoint(model.Editor.AccessPointId, model.Editor.Site, model.Editor.Department, model.Editor.Name) {Description = model.Editor.Description};
            var result = await _registerAccessPointRequest.Request(new RegisterAccessPoint(accessPoint));
            if (!result.Succeded)
            {
                ModelState.AddModelError(string.Empty, result.Fault.Summary);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        private async Task Initialize(AccessPointIndexViewModel model)
        {
            var appUser = HttpContext.GetApplicationUser();
            var accessPointsTask = _listAccessPointsRequest.Request(ListCommand.Default);
            var departmentsTask = _listDepartmentsRequest.Request(ListCommand.Default);

            await Task.WhenAll(accessPointsTask, departmentsTask);

            model.AccessPoints = accessPointsTask.Result.AccessPoints;
            model.Editor.SiteDepartments = departmentsTask
                .Result
                .Departments
                .GroupBy(x => Tuple.Create(x.Site, x.SiteName))
                .Select(x => Tuple.Create(x.Key.Item1, x.Key.Item2, x.Select(t => t.DepartmentName).ToArray()))
                .ToList();
        }
    }
}