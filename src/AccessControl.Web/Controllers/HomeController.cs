using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessControl.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return !User.Identity.IsAuthenticated ? View("Description") : View("Dashboard");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}