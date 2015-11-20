using System.Web;
using AccessControl.Web.Models.Account;
using Microsoft.AspNet.Identity.Owin;

namespace AccessControl.Web.Services
{
    public static class HttpContextExtensions
    {
        public static ApplicationUser GetApplicationUser(this HttpContextBase context)
        {
            return context.User != null && context.User.Identity.IsAuthenticated ? new ApplicationUser(context.User.Identity) : new ApplicationUser();
        }

        public static ApplicationSignInManager GetSignInManager(this HttpContextBase context)
        {
            return context.GetOwinContext().Get<ApplicationSignInManager>();
        }

        public static ApplicationUserManager GetUserManager(this HttpContextBase context)
        {
            return context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
    }
}