using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using OneVietnam.BLL;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam
{
    [Authorize]    
    public class OneHub : Hub
    {        
        public ApplicationUserManager UserManager { get; private set; }
        public MessageManager MessageManager { get; set; }

        public OneHub(ApplicationUserManager userManager,MessageManager messageManager)
        {
            UserManager = userManager;
            MessageManager = messageManager;
        }
        public async Task SendChatMessage(string receiveId, string message)
        {            

            var sendName = Context.User.Identity.Name;
            var receiveUser = await UserManager.FindByIdAsync(receiveId);
            var connecting = receiveUser.Connections.Any(c => c.Connected == true);
            var connection = receiveUser.Connections;
            if (connection != null)
            {
                foreach (var conn in connection.Where(conn => conn.Connected == true))
                {
                    // call client's javascript function 
                    Clients.Client(conn.ConnectionId).addChatMessage(sendName + ": " + message);
                }
            }            
            //Add Messages to database
            var mes = new Message
            {
                ReceiverId = receiveId,
                SenderId = Context.User.Identity.GetUserId(),
                Content = message,
                CreatedDate = DateTimeOffset.UtcNow
            };
             await MessageManager.CreateAsync(mes).ConfigureAwait(false);
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