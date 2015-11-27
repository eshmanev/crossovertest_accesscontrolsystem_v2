using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Models.Test;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IRequestClient<IListAccessPoints, IListAccessPointsResult> _listAccessPointsRequest;
        private readonly IRequestClient<IListUsersBiometric, IListUsersBiometricResult> _listUsersRequest;

        public TestController(IRequestClient<IListUsersBiometric, IListUsersBiometricResult> listUsersRequest,
                              IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest)
        {
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(listAccessPointsRequest != null);

            _listUsersRequest = listUsersRequest;
            _listAccessPointsRequest = listAccessPointsRequest;
        }

        public async Task<ActionResult> Index()
        {
            var usersTask = _listUsersRequest.Request(ListCommand.Default);
            var accessPointsTask = _listAccessPointsRequest.Request(ListCommand.Default);

            await Task.WhenAll(usersTask, accessPointsTask);

            var model = new TestViewModel();
            return View(model);
        }
    }
}