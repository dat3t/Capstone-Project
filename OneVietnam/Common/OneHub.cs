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
        //public static HttpContext hc = new HttpContext();

        public ApplicationUserManager Manager { get; private set; }

        public OneHub(ApplicationUserManager manager)
        {
            Manager = manager;
        }
        public async Task SendChatMessage(string receiveId, string message)
        {                        
            var sendName = Context.User.Identity.Name;            
            var receiveUser = await Manager.FindByIdAsync(receiveId);
            var connecting = receiveUser.Connections.Any(c => c.Connected == true);
            var connection = receiveUser.Connections;
            if (connection!=null)
            {
                foreach (var conn in connection.Where(conn => conn.Connected==true))
                {
                    Clients.Client(conn.ConnectionId).addChatMessage(sendName + ": " + message);
                }
            }
            //get sender's userId
            var sendId = Context.User.Identity.GetUserId();
            //Create sending message to store in sender User
            var sendMes = new Message(receiveId, message, (int)MessageType.Send);
            await Manager.AddMessage(sendId, sendMes);
            //Create receiving message to store in receiver user
            var receiveMes = new Message(sendId, message, (int)MessageType.Receive);
            receiveUser.AddMessage(receiveMes);
            await Manager.UpdateAsync(receiveUser);
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
            await Manager.AddConnection(userId, conn);                        
            await base.OnConnected(); 
        }
        public override async Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.GetUserId();
            var connectionId = Context.ConnectionId;
            await Manager.DisConnection(userId, connectionId);                        
            await base.OnDisconnected(stopCalled);
        }
    }
}