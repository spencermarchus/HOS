using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HOS.Startup))]
namespace HOS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
