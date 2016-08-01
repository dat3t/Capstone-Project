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
                var con = new ConversationModel();
                con.Id = conversations[i].Key;
                con.FriendName = friend.UserName;
                con.Avatar = friend.Avatar;
                if (DateTimeOffset.Now.Year == conversations[i].Value.UpdatedDate.LocalDateTime.Year &&
                    DateTimeOffset.Now.Day == conversations[i].Value.UpdatedDate.LocalDateTime.Day)
                {
                    con.UpdatedDate = conversations[i].Value.UpdatedDate.LocalDateTime.ToShortTimeString();
                }
                else
                {
                    con.UpdatedDate = conversations[i].Value.UpdatedDate.LocalDateTime.ToString();
                }
                con.LastestMessage = conversations[i].Value.LastestMessage.Truncate(Constants.MessagePreviewMaxLength);
                con.LastestType = conversations[i].Value.LastestType;
                con.Seen = conversations[i].Value.Seen;                
                conversationList.Add(con);
            }

            return Json(conversationList, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetNotifications()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var notifications = user.Notifications;
            int updatedCount = 0;
            List<NotificationModel> list = new List<NotificationModel>();
            for (int i = 0; i < notifications.Count - 1; i++)
            {
                var not = new NotificationModel
                {
                    Id=notifications.Values[i].Id,
                    Url = notifications.Values[i].Url,
                    Avatar = notifications.Values[i].Avatar,
                    Description = notifications.Values[i].Description,
                    Seen = notifications.Values[i].Seen
                };
                if (notifications.Values[i].Seen == false)
                {
                    notifications.Values[i].Seen = true;
                    updatedCount++;
                }
                if (DateTimeOffset.Now.Year == notifications.Values[i].CreatedDate.LocalDateTime.Year &&
                    DateTimeOffset.Now.Day == notifications.Values[i].CreatedDate.LocalDateTime.Day)
                {
                    not.CreatedDate = notifications.Values[i].CreatedDate.LocalDateTime.ToShortTimeString();
                }
                else
                {
                    not.CreatedDate = notifications.Values[i].CreatedDate.LocalDateTime.ToString();
                }
                list.Add(not);
            }
            if (updatedCount > 0)
            {
                await UserManager.UpdateAsync(user);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public async Task<bool> RemoveNotificationById(string id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var result = user.Notifications.Remove(id);
            await UserManager.UpdateAsync(user);
            return result;
        }

        public async Task<int> GetNotificationsNo()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return user.CountUnReadNotifications();
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
            var result = user.Conversations.Remove(id);
            await UserManager.UpdateAsync(user);
            return result;
        }
    }
}