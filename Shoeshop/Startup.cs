using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shoeshop.Startup))]
namespace Shoeshop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
