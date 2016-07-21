using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LineUp_Website.Startup))]
namespace LineUp_Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
