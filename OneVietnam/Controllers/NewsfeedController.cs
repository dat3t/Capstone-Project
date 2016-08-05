﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;
using Facebook;

namespace OneVietnam.Controllers
{
    [System.Web.Mvc.Authorize]
    public class NewsfeedController : Controller
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
       private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
      private  CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        public static bool CreatedPost = false;
        public static PostViewModel PostView;        

        public NewsfeedController()
        {
        }

        public NewsfeedController(ApplicationUserManager userManager)
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
        //public List<Tag> TagList => TagManager.FindAllAsync().Result

        private ReportManager _reportManager;
        public ReportManager ReportManager
        {
            get
            {
                return _reportManager ?? HttpContext.GetOwinContext().Get<ReportManager>();
            }
            private set { _reportManager = value; }
        }

        //public List<Icon> IconList
        //{
        //    get
        //    {
        //        var icons = await IconManager.GetIconPostAsync();
        //        return icons;
        //    }            
        //}

        public List<Icon> GenderIcon
        {
            get
            {
                var gender = IconManager.GetIconGender().Result;
                return gender;
            }
        }

        public async Task _CreatePost()
        {            
            var tagList = await TagManager.FindAllAsync(false);
            if (tagList != null)
            {
                ViewData["TagList"] = tagList;
            }                                               
            var iconList = await IconManager.GetIconPostAsync();
            if (iconList != null)
            {
                ViewData["PostTypes"] = iconList;
            }            
        }

        private HttpFileCollectionBase _illustrationList;

        public void GetIllustrations()
        {
            _illustrationList = Request.Files;
            Session["Illustrations"] = _illustrationList;
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(CreatePostViewModel p)
        {
            HttpFileCollectionBase files = Request.Files;
            ViewData.Clear();
            var post = new Post(p)
            {
                CreatedDate = System.DateTime.Now,
                UserId = User.Identity.GetUserId()
            };
            var tagList = await PostManager.AddAndGetAddedTags(Request, TagManager, "TagsInput");
            _illustrationList = (HttpFileCollectionBase)Session["Illustrations"];
            var illList = await PostManager.GetIllustration(_illustrationList, post.Id);
            Session["Illustrations"] = null;
            _illustrationList = null;
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
            return RedirectToAction("Index", "Newsfeed");
        }        

        public const int RecordsPerPage = 60;

        public async Task<ActionResult> _AdminPost()
        {
            List<PostViewModel> list = new List<PostViewModel>();
            var posts = await PostManager.FindAllActiveAdminPostAsync();
            foreach (var post in posts)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(post.UserId);
                list.Add(new PostViewModel(post, user.UserName, user.Avatar));

            }
            return PartialView("_AdminPost", list);
        }
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? pageNum)
        {            
            var tagList = await TagManager.FindAllAsync(false);
            if (tagList != null)
            {
                ViewData["TagList"] = tagList;
            }
            var iconList = await IconManager.GetIconPostAsync();
            if (iconList != null)
            {
                ViewData["PostTypes"] = iconList;
            }
            pageNum = pageNum ?? 1;
            ViewBag.IsEndOfRecords = false;

            BaseFilter filter;
            List<Post> posts;
            List<PostViewModel> list = new List<PostViewModel>();
            if (Request.IsAjaxRequest())
                {
                filter = new BaseFilter { CurrentPage = pageNum.Value };
                posts = await PostManager.FindAllDescenderAsync(filter);

                if (posts.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
                    foreach (var post in posts)
                    {
                        ApplicationUser user = await UserManager.FindByIdAsync(post.UserId);
                    list.Add(new PostViewModel(post, user.UserName, user.Avatar));

                    }
                //ViewBag.IsEndOfRecords = (posts.Any()) && ((pageNum.Value * RecordsPerPage) >= posts.Last().Key);
                return PartialView("_PostRow", list);
                }
        
            filter = new BaseFilter { CurrentPage = pageNum.Value };
            posts = await PostManager.FindAllDescenderAsync(filter);
            if (posts.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
            foreach (var post in posts)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(post.UserId);
                //don't load post of deleted user
                if (user?.DeletedFlag == false && user?.LockedFlag==false)
                {
                    list.Add(new PostViewModel(post, user.UserName, user.Avatar));
                }
            }
            ViewBag.Posts = list;
            return View();
        }
        /// <summary>
        /// Get posts for pagenum
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns>List Post></returns>
        [AllowAnonymous]
        public async Task<ActionResult> ShowPost(string Id)
        {
            Post post = await PostManager.FindByIdAsync(Id);
            if (post != null)
            {
                ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
                if (postUser != null)
                {

                    PostViewModel showPost = new PostViewModel(post, postUser.UserName, postUser.Avatar);

                    return View(showPost);
                }
            }
            return View();
        }
        public async Task<List<Post>> GetRecordsForPage(int pageNum)
        {
            //Dictionary<int, Post> posts = (Session["Posts"] as Dictionary<int, Post>);

            //int from = (pageNum * RecordsPerPage);
            //int to = from + RecordsPerPage;

            //return posts
            //    .Where(x => x.Key > from && x.Key <= to)
            //    .OrderBy(x => x.Key)
            //    .ToDictionary(x => x.Key, x => x.Value);
            var filter = new BaseFilter { CurrentPage = pageNum };
            return await PostManager.FindAllAsync(filter);
        }
        [AllowAnonymous]
        public async Task<ActionResult> _ShowPost(string postId)
        {
            //List<Post> list = await PostManager.FindByUserId(User.Identity.GetUserId());
            Post post = await PostManager.FindByIdAsync(postId);
            if (post != null)
            {
                ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
                if (postUser != null)
                {

                    PostViewModel showPost = new PostViewModel(post, postUser.UserName, postUser.Avatar);

                    return PartialView(showPost);
                }
            }
            return PartialView();
        }
        [AllowAnonymous]
        public async Task<ActionResult> ShowPostDetail(string postId)
        {
            var tagList = await TagManager.FindAllAsync(false);
            if (tagList != null)
                    {
                ViewData["TagList"] = tagList;
                    }
            var iconList = await IconManager.GetIconPostAsync();
            if (iconList != null)
                    {
                ViewData["PostTypes"] = iconList;
                    }
            Post post = await PostManager.FindByIdAsync(postId);
            if (post != null)
            {
                    ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
                    if (postUser != null)
                    {

                    PostViewModel showPost = new PostViewModel(post, postUser.UserName, postUser.Avatar);

                        return View(showPost);
                    }
                }
            return View();
        }

        public JsonResult GetCommentor(string commentid)
        {     
            var fb = new FacebookClient(Constants.accessTokenFacebook);
            dynamic commentInfo = fb.Get(commentid);                                            
            string id = commentInfo["from"]["id"];
            dynamic userInfo = fb.Get(id+"?fields=picture");            
            var commentor = new CommentorViewModel
            {
                Avatar = userInfo["picture"]["data"]["url"],
                Username = commentInfo["from"]["name"]
            };

            return Json(commentor, JsonRequestBehavior.AllowGet);
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
            return RedirectToAction("DeletePost", "Newsfeed", new { postId = strPostId });
        }

        //[HttpPost]
        //public async Task ReportPost(string userId, string postId, string description)
        //{
        //    Report report = new Report(userId, postId, description);
        //    await ReportManager.CreateAsync(report);
        //    //TODO send notification to Mod
        //}

        [HttpPost]
        public async Task<ActionResult> ReportPost(ReportViewModal model)
        {
            Report report = new Report(model);
            await ReportManager.CreateAsync(report);
            return PartialView("../Newsfeed/_Report", new ReportViewModal(model.Id, model.UserId));
        }

        [System.Web.Mvc.Authorize]        
        public async Task<ActionResult> EditPost(string postId)
        {
           
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(postId);
            await blobContainer.CreateIfNotExistsAsync();

            List<Uri> allBlobs = new List<Uri>();

            foreach (IListBlobItem blob in blobContainer.ListBlobs())
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                    allBlobs.Add(blob.Uri);
            }
            ViewData["Blobs"] = allBlobs;
            var tagList = await TagManager.FindAllAsync(false);
            if (tagList != null)
                    {
                ViewData["TagList"] = tagList;
                    }
            var iconList = await IconManager.GetIconPostAsync();
            if (iconList != null)
                    {
                ViewData["PostTypes"] = iconList;
                    }
            Post post = await PostManager.FindByIdAsync(postId);
                    PostViewModel showPost = new PostViewModel(post);
                    return View(showPost);
                }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(PostViewModel pPostView)
        {                                                
            ViewData.Clear();
            var tagList = await PostManager.AddAndGetAddedTags(Request, TagManager, "TagsInput");
            if (tagList != null)
            {
                pPostView.Tags = tagList;
            }
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(pPostView.Id);

            List<Illustration> illList=new List<Illustration>();
         
            _illustrationList = (HttpFileCollectionBase)Session["Illustrations"];
            if (_illustrationList.Count > 0) { 
                for (int i = 0; i < _illustrationList.Count; i++)
            {

                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(_illustrationList[i].FileName));
                await blob.UploadFromStreamAsync(_illustrationList[i].InputStream);

            }
                
            }
            var blobList = blobContainer.ListBlobs();
            illList = new List<Illustration>();
            foreach (var blob in blobList)
            {
                Illustration newIll = new Illustration(blob.Uri.ToString());
                illList.Add(newIll);
            }
            if (illList != null)
            {
                pPostView.Illustrations = illList;
            }
            Post post = new Post(pPostView);           
            await PostManager.UpdateAsync(post);
            return RedirectToAction("ShowPost", "Newsfeed", new { pPostView.Id });
        }
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "Admin")]        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAdminPost(AdminPostViewModel pPostView)
        {
            ViewData.Clear();            
            var illList = await PostManager.GetIllustration(_illustrationList, pPostView.Id);
            if (illList != null)
            {
                pPostView.Illustrations = illList;
            }
            Post post = new Post(pPostView);
            await PostManager.UpdateAsync(post);
            return RedirectToAction("ShowPostDetail", "Newsfeed", new { postId = pPostView.Id });
        }

        [System.Web.Mvc.Authorize]        
        public async Task<ActionResult> DeletePost(string postId)
        {
            Post post = await PostManager.FindByIdAsync(postId);
            post.DeletedFlag = true;                                             
            await PostManager.UpdateAsync(post);
            return RedirectToAction("Index", "Newsfeed");
        }
        [HttpPost]
        public async Task<ActionResult> DeleteImage(string name, string id)
        {
            try
            {
                Uri uri = new Uri(name);
                string filename = Path.GetFileName(uri.LocalPath);
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer blobContainer = blobClient.GetContainerReference(id);

                var blob = blobContainer.GetBlockBlobReference(filename);
                await blob.DeleteIfExistsAsync();
                return RedirectToAction("EditPost", "Newsfeed", new { postId = id });



            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
        public class MyHub : Hub
        {
            public override Task OnConnected()
            {
                if (NewsfeedController.CreatedPost)
                {
                    var javaScriptSerializer = new JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(NewsfeedController.PostView);
                    Clients.Others.loadNewPost(jsonString);
                }
                return base.OnConnected();
            }
        }                
    }
}