using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Majstic.Startup))]
namespace Majstic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
