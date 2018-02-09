using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(diplom_2.Startup))]
namespace diplom_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
