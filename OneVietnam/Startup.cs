using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OneVietnam.Startup))]
namespace OneVietnam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
