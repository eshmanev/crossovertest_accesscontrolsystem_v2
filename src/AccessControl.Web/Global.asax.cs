using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AccessControl.Contracts;
using MassTransit;
using Microsoft.Practices.Unity;

namespace AccessControl.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IBusControl _bus;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.ConfigureContainer();

            _bus = BusConfig.Configure(UnityConfig.Container);
            _bus.Start();
        }

        protected void Application_Stop()
        {
            _bus.Stop();
            UnityConfig.Container.Dispose();
        }
    }
}