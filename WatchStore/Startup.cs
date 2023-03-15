using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WatchStore.Startup))]
namespace WatchStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
