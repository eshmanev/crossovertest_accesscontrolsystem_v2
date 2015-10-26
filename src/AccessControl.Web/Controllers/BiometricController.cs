using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Web.Services;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class BiometricController : Controller
    {
        private readonly IRequestClient<IListUsersExtended, IListUsersExtendedResult> _listUsersRequest;
        private readonly IRequestClient<IUpdateUserBiometric, IVoidResult> _updateBiometricRequest;

        public BiometricController(IRequestClient<IListUsersExtended, IListUsersExtendedResult> listUsersRequest, IRequestClient<IUpdateUserBiometric, IVoidResult> updateBiometricRequest)
        {
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(updateBiometricRequest != null);

            _listUsersRequest = listUsersRequest;
            _updateBiometricRequest = updateBiometricRequest;
        }

        // GET: Biometric
        public async Task<ActionResult> Index()
        {
            var user = HttpContext.GetApplicationUser();
            var result = await _listUsersRequest.Request(new ListUsersExtended(user.Site, user.Department));
            return View(result.Users);
        }

        [HttpPost]
        public async Task<JsonResult> UserHash(string userName, string hash)
        {
            var result = await _updateBiometricRequest.Request(new UpdateUserBiometric(userName, hash));
            return Json(result);
        }
    }
}