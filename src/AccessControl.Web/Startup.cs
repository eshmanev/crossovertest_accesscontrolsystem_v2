﻿using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using AccessControl.Web.Owin;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccessControl.Web.Startup))]
namespace AccessControl.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseUnityMiddleware(UnityConfig.Container);
            ConfigureAuth(app);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }
    }
}
