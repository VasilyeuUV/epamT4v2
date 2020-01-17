using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pl_ReactJSAspNetWebApp.Startup))]
namespace pl_ReactJSAspNetWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
