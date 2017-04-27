using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pyramid.Startup))]
namespace Pyramid
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
