using System.Reflection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using OneVietnam.BLL;
using OneVietnam.DAL;
using OneVietnam.DTL;
using Owin;

[assembly: OwinStartupAttribute(typeof(OneVietnam.Startup))]
[assembly: OwinStartup(typeof(OneVietnam.Startup))]
namespace OneVietnam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var context = ApplicationIdentityContext.Create();
            var postManager = new ApplicationUserManager(new UserStore(context.Users));       
            var messageManager = new MessageManager(new MessageStore(context.Messages));                     
            GlobalHost.DependencyResolver.Register(
            typeof(OneHub),
            () => new OneHub(postManager,messageManager));
            app.MapSignalR();
        }
    }
}