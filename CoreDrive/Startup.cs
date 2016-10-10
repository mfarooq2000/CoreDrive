using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoreDrive.Startup))]
namespace CoreDrive
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
