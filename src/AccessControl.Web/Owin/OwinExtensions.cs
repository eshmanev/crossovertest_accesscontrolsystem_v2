using System.Diagnostics.Contracts;
using Microsoft.Practices.Unity;
using Owin;

namespace AccessControl.Web.Owin
{
    public static class OwinExtensions
    {
        private const string Key = "Unity:Middleware";

        public static IAppBuilder UseUnityMiddleware(this IAppBuilder appBuilder, IUnityContainer container)
        {
            Contract.Requires(appBuilder !=null);
            Contract.Requires(container != null);

            if (appBuilder.Properties.ContainsKey(Key))
                return appBuilder;

            appBuilder.Use<RequestLifetimeMiddleware>(container);
            appBuilder.Properties[Key] = true;
            return appBuilder;
        }
    }
}