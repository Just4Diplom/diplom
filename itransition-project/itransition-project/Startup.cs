using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(itransition_project.Startup))]
namespace itransition_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
