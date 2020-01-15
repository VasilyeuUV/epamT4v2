using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(plAspNetWebApp.Startup))]
namespace plAspNetWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
