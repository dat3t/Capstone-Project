using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class PostController : Controller
    {
        public static bool CreatedPost = false;
        public static ShowPostViewModel PostView;

        public PostController()
        {
        }

        public PostController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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

        public ActionResult CreatePost()
        {
            return View(new CreatePostViewModel());
        }
        //DEMO
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(CreatePostViewModel p)
        {
            var post = new Post(p) { Username = User.Identity.Name };
            await UserManager.AddPostAsync(User.Identity.GetUserId(), post);
            CreatedPost = true;
            PostView = new ShowPostViewModel(post);
            return RedirectToAction("ShowCreatedPost");
        }

        public async Task<ActionResult> ShowPost()
        {

            List<Post> list = await UserManager.GetPostsAsync(User.Identity.GetUserId());
            List<ShowPostViewModel> pViewList = list.Select(post => new ShowPostViewModel(post)).ToList();
            return View(pViewList);
        }

        public ActionResult ShowCreatedPost()
        {
            return View(PostView);
        }


        public class MyHub : Hub
        {
            public override Task OnConnected()
            {
                if (PostController.CreatedPost)
                {
                    var javaScriptSerializer = new JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(PostController.PostView);
                    Clients.Others.loadNewPost(jsonString);
                }
                return base.OnConnected();
            }
        }
    }
}