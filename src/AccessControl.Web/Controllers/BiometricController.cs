using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Mvc;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using AccessControl.Web.Services;
using MassTransit;

namespace AccessControl.Web.Controllers
{
    public class BiometricController : Controller
    {
        private readonly IRequestClient<IListBiometricInfo, IUserWithBiometric[]> _listBioInfo;

        public BiometricController(IRequestClient<IListBiometricInfo, IUserWithBiometric[]> listBioInfo)
        {
            Contract.Requires(listBioInfo != null);
            _listBioInfo = listBioInfo;
        }

        // GET: Biometric
        public async Task<ActionResult> Index()
        {
            var user = HttpContext.GetApplicationUser();
            var biometricInfo = await _listBioInfo.Request(new ListBiometricInfo(user.Site, user.Department));
            return View();
        }
    }
}