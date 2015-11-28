using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    [Authorize]
    public class BiometricController : Controller
    {
        private readonly IRequestClient<IListUsersBiometric, IListUsersBiometricResult> _listUsersRequest;
        private readonly IRequestClient<IUpdateUserBiometric, IVoidResult> _updateBiometricRequest;

        public BiometricController(IRequestClient<IListUsersBiometric, IListUsersBiometricResult> listUsersRequest,
                                   IRequestClient<IUpdateUserBiometric, IVoidResult> updateBiometricRequest)
        {
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(updateBiometricRequest != null);

            _listUsersRequest = listUsersRequest;
            _updateBiometricRequest = updateBiometricRequest;
        }

        // GET: Biometric
        public async Task<ActionResult> Index()
        {
            var result = await _listUsersRequest.Request(ListCommand.WithoutParameters);
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