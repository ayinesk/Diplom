using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoadDist.Startup))]
namespace LoadDist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
