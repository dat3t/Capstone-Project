using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using OneVietnam.BLL;
using OneVietnam.Common;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam
{
    [Authorize]    
    public class OneHub : Hub
    {        
        public ApplicationUserManager UserManager { get; private set; }

        public OneHub(ApplicationUserManager userManager)
        {
            UserManager = userManager;            
        }
        public async Task SendChatMessage(string friendId, string message)
        {
            var name = Context.User.Identity.Name;
            var id = Context.User.Identity.GetUserId();  
            var friend = await UserManager.FindByIdAsync(friendId);
            var connecting = friend.Connections.Any(c => c.Connected == true);
            var connection = friend.Connections;
            if (connection != null)
            {
                foreach (var conn in connection.Where(conn => conn.Connected == true))
                {
                    // call client's javascript function 
                    Clients.Client(conn.ConnectionId).addChatMessage(id,name,message);
                }
            }
            // Store Message To Database
            await UserManager.AddMessage(Context.User.Identity.GetUserId(), friendId, message);            
        }
        public override async Task OnConnected()
        {            
            var userId = Context.User.Identity.GetUserId();
            var conn = new Connection
            {
                ConnectionId = Context.ConnectionId,
                Connected = true,
                UserAgent = Context.Request.Headers["User-Agent"]
            };
            await UserManager.AddConnection(userId, conn);                        
            await base.OnConnected(); 
        }
        public override async Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.GetUserId();
            var connectionId = Context.ConnectionId;
            await UserManager.DisConnection(userId, connectionId);                        
            await base.OnDisconnected(stopCalled);
        }
    }
}