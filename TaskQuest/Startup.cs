using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskQuest.Startup))]
namespace TaskQuest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
