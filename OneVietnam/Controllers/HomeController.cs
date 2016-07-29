using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public async Task<JsonResult> GetConversationById(string friendId)
        {
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var friendConversation = user.Conversations[friendId];
            if (friendConversation == null) return null;
            if (!friendConversation.Seen)
            {
                friendConversation.Seen = true;
                await UserManager.UpdateAsync(user);
            }            
            var messagesList = friendConversation.MessageList;
            return Json(messagesList, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetConversations()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var conversations = user.Conversations.OrderByDescending(c => c.Value).ToList();
            var conversationList = new List<ConversationModel>();            
            for (var i = 0; i < conversations.Count(); i++)
            {
                var friend = await UserManager.FindByIdAsync(conversations[i].Key);                
                var con = new ConversationModel
                {
                    Id = conversations[i].Key,
                    FriendName = friend.UserName,
                    Avatar = friend.Avatar,
                    UpdatedDate = conversations[i].Value.UpdatedDate,
                    LastestMessage = conversations[i].Value.LastestMessage.Truncate(Constants.MessagePreviewMaxLength),
                    LastestType = conversations[i].Value.LastestType,
                    Seen= conversations[i].Value.Seen
                };
                conversationList.Add(con);
            }

            return Json(conversationList, JsonRequestBehavior.AllowGet);
        }        

        public async Task<JsonResult> GetMessageNo(string id)
        {
            var user = await UserManager.FindByIdAsync(id);            
            int count = user.CountUnReadConversations();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public async Task<bool> RemoveConversationById(string id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var result =  user.Conversations.Remove(id);
            await UserManager.UpdateAsync(user);
            return result;
        }
    }
}