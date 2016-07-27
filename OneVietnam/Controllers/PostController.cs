
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OneVietnam.BLL;
using OneVietnam.Common;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class PostController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public static bool CreatedPost = false;
        public static PostViewModel PostView;        

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

        private PostManager _postManager;
        public PostManager PostManager
        {
            get
            {
                return _postManager ?? HttpContext.GetOwinContext().Get<PostManager>();
            }
            private set { _postManager = value; }
        }
        private TagManager _tagManager;
        public TagManager TagManager
        {
            get
            {
                return _tagManager ?? HttpContext.GetOwinContext().Get<TagManager>();
            }
            private set { _tagManager = value; }
        }        

        private IconManager _iconManager;
        public IconManager IconManager
        {
            get
            {
                return _iconManager ?? HttpContext.GetOwinContext().Get<IconManager>();
            }
            private set { _iconManager = value; }
        }
        public List<Tag> TagList => TagManager.FindAllAsync().Result;

        private ReportManager _reportManager;
        public ReportManager ReportManager
        {
            get
            {
                return _reportManager ?? HttpContext.GetOwinContext().Get<ReportManager>();
            }
            private set { _reportManager = value; }
        }

        public List<Icon> IconList
        {
            get
            {
                var icons = IconManager.GetIconPostAsync();
                return icons;
            }            
        }

        public void _CreatePost()
        {            
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }                                               
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }            
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(CreatePostViewModel p)
        {
            ViewData.Clear();
            var post = new Post(p)
            {
                CreatedDate = System.DateTime.Now,
                UserId = User.Identity.GetUserId()
            };
            var tagList = await PostManager.AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = await PostManager.GetIllustration(Request, "createPost", post.Id);
            if (tagList != null)
            {
                post.Tags = tagList;
            }

            if (illList != null)
            {
                post.Illustrations = illList;
            }            
            await PostManager.CreateAsync(post);
            CreatedPost = true;
            PostView = new PostViewModel(post);
            return RedirectToAction("NewFeeds", "Post");
        }        

        public const int RecordsPerPage = 60;

        public async Task<ActionResult> Newfeeds(int? pageNum)
        {            
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }
            pageNum = pageNum ?? 1;
            ViewBag.IsEndOfRecords = false;

            BaseFilter filter;
            List<Post> posts;
            List<PostViewModel> list;
            if (Request.IsAjaxRequest())
                {
                filter = new BaseFilter {CurrentPage = pageNum.Value};
                posts = await PostManager.FindAllAsync(filter);
                if (posts.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
                list = posts.Select(p => new PostViewModel(p)).ToList();                
                //ViewBag.IsEndOfRecords = (posts.Any()) && ((pageNum.Value * RecordsPerPage) >= posts.Last().Key);
                return PartialView("_PostRow", list);
                }
            //else
            //{
            //    // LoadAllPostsToSession
            //    List<Post> list = await PostManager.FindAllPostsAsync();
            //    var posts = list;
            //    int postIndex = 1;
            //    Session["Posts"] = posts.ToDictionary(x => postIndex++, x => x);
            
            //    ViewBag.Posts = GetRecordsForPage(pageNum.Value);
            //    return View();
            //}
            filter = new BaseFilter { CurrentPage = pageNum.Value };
            posts = await PostManager.FindAllAsync(filter);
            if (posts.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
            list = posts.Select(p => new PostViewModel(p)).ToList();
            ViewBag.Posts = list;
            return View();
        }
        /// <summary>
        /// Get posts for pagenum
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns>List Post></returns>
        public async Task<List<Post>> GetRecordsForPage(int pageNum)
        {
            //Dictionary<int, Post> posts = (Session["Posts"] as Dictionary<int, Post>);

            //int from = (pageNum * RecordsPerPage);
            //int to = from + RecordsPerPage;

            //return posts
            //    .Where(x => x.Key > from && x.Key <= to)
            //    .OrderBy(x => x.Key)
            //    .ToDictionary(x => x.Key, x => x.Value);
            var filter = new BaseFilter {CurrentPage = pageNum};
            return await PostManager.FindAllAsync(filter);
        }
        [HttpPost]

        public async Task<ActionResult> ShowPost()
        {
            //List<Post> list = await PostManager.FindByUserId(User.Identity.GetUserId());
            List<Post> list = await PostManager.FindAllAsync(false);
            List<PostViewModel> pViewList = list.Select(post => new PostViewModel(post)).ToList();
            return View(pViewList);
        }

        public async Task<ActionResult> ShowPostDetail(string postId)
        {
                    if (TagList != null)
                    {
                        ViewData["TagList"] = TagList;
                    }
                    if (IconList != null)
                    {
                        ViewData["PostTypes"] = IconList;
                    }
            Post post = await PostManager.FindByIdAsync(postId);
            if (post != null)
            {
                    ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
                    if (postUser != null)
                    {

                        PostViewModel showPost = new PostViewModel(post, postUser.UserName);

                        return View(showPost);
                    }
                }
            return View();
        }

        [HttpPost]
        public ActionResult ShowPostDetail(PostViewModel pPostView)

        {            
            ViewData.Clear();
            string strPostId = "";
            if (Request.Form.Count > 0)
            {
                strPostId = Request.Form["PostId"];

            }                                    
            return RedirectToAction("DeletePost", "Post", new { postId = strPostId });
        }
  
        [HttpPost]
        public async Task ReportPost(string userId, string postId, string description)
        {
            Report report = new Report(userId, postId, description);
            await ReportManager.CreateAsync(report);
            //TODO send notification to Mod
        }

        public async Task<ActionResult> EditPost(string postId)
        {
                    if (TagList != null)
                    {
                        ViewData["TagList"] = TagList;
                    }
                    if (IconList != null)
                    {
                        ViewData["PostTypes"] = IconList;
                    }
            Post post = await PostManager.FindByIdAsync(postId);
                    PostViewModel showPost = new PostViewModel(post);
                    return View(showPost);
                }

        [HttpPost]
        public async Task<ActionResult> EditPost(PostViewModel pPostView)
        {                        
            ViewData.Clear();
            string strPostId = "";
            if (Request.Form.Count > 0)
            {
                strPostId = Request.Form["PostId"];
            }
            ViewData.Clear();
            var tagList = await PostManager.AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = await PostManager.GetIllustration(Request, "createPost", pPostView.Id);
            if (tagList != null)
            {
                pPostView.Tags = tagList;
            }

            if (illList != null)
            {
                pPostView.Illustrations = illList;
            }
            Post post = new Post(pPostView);           
            await PostManager.UpdateAsync(post);
            return RedirectToAction("ShowPostDetail", "Post", new { postId = strPostId });
        }

        public async Task<ActionResult> DeletePost(string postId)
        {
            Post post = await PostManager.FindByIdAsync(postId);
            post.DeletedFlag = true;                                             
            await PostManager.UpdateAsync(post);
            return RedirectToAction("CreatePost", "Post");
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