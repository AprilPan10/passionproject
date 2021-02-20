using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mypassionproject.Startup))]
namespace mypassionproject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
