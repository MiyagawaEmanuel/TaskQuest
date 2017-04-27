using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TastQuest.Startup))]
namespace TastQuest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
