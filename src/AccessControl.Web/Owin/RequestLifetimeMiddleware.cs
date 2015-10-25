using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Practices.Unity;

namespace AccessControl.Web.Owin
{
    public class RequestLifetimeMiddleware : OwinMiddleware
    {
        private readonly IUnityContainer _container;

        public RequestLifetimeMiddleware(OwinMiddleware next, IUnityContainer container)
            : base(next)
        {
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);
        }
    }
}