using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Services;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class AccessPointController : Controller
    {
        private readonly IRequestClient<IListAccessPoints, IListAccessPointsResult> _listAccessPointsRequest;

        public AccessPointController(IRequestClient<IListAccessPoints, IListAccessPointsResult> listAccessPointsRequest)
        {
            Contract.Requires(listAccessPointsRequest != null);
            _listAccessPointsRequest = listAccessPointsRequest;
        }

        public async Task<ActionResult> Index()
        {
            var appUser = HttpContext.GetApplicationUser();
            var result = await _listAccessPointsRequest.Request(new ListAccessPoints(appUser.Site, appUser.Department));
            return View(result.AccessPoints);
        }
    }
}