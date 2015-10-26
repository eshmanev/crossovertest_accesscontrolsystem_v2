using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts;
using AccessControl.Contracts.CodeContracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Services;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    public class BiometricController : Controller
    {
        private readonly IRequestClient<IListUsersExtended, IListUsersExtendedResult> _listUsersRequest;
        private readonly IBus _bus;

        public BiometricController(IRequestClient<IListUsersExtended, IListUsersExtendedResult> listUsersRequest, IBus bus)
        {
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(bus != null);

            _listUsersRequest = listUsersRequest;
            _bus = bus;
        }

        // GET: Biometric
        public async Task<ActionResult> Index()
        {
            var user = HttpContext.GetApplicationUser();
            var result = await _listUsersRequest.Request(new ListUsersExtended(user.Site, user.Department));
            return View(result.Users);
        }

        [HttpPost]
        public async Task UserHash(string userName, string hash)
        {
            await _bus.Publish(new UpdateUserBiometric(userName, hash));
        }
    }
}