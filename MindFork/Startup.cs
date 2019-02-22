using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MindFork.Startup))]
namespace MindFork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
