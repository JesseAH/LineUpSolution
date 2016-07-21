using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LineUpApp.Startup))]
namespace LineUpApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
