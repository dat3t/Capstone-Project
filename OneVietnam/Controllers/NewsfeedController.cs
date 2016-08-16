
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
using System.Collections;
using System.Configuration;
using System.Security.Claims;
using OneVietnam.Common;
using Microsoft.AspNet.SignalR.Hubs;
namespace OneVietnam.Controllers
{
    [System.Web.Mvc.Authorize]
    public class NewsfeedController : Controller
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        private static readonly CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
        private readonly CloudBlobClient _blobClient = StorageAccount.CreateCloudBlobClient();        
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

        private ReportManager _reportManager;
        public ReportManager ReportManager
        {
            get
            {
                return _reportManager ?? HttpContext.GetOwinContext().Get<ReportManager>();
            }
            private set { _reportManager = value; }
        }
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
            Session.Clear();
            Session.Add("Illustrations", _illustrationList);
            Console.WriteLine("Go Get OK");            
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
            _illustrationList = (HttpFileCollectionBase)Session["Illustrations"];
            var illList = await PostManager.AzureUploadAsync(_illustrationList, post.Id);
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

            if (post.PostType == (int) PostTypeEnum.AdminPost)
            {
                var hubContext =  GlobalHost.ConnectionManager.GetHubContext<OneHub>();
                var avatar = ((ClaimsIdentity) User.Identity).FindFirst("Avatar").Value;                
                var description = Constants.AdminNotification + "\"" +
                                  post.Title + "\"";
                var notice = new Notification(Url.Action("ShowPostDetailPage", "Newsfeed", new { post.Id }), avatar, description);
                await UserManager.PushAdminNotificationToAllUsersAsync(notice);
                await hubContext.Clients.All.pushNotification();
            }
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
        public async Task<ActionResult> Index(int? pageNum,int? filterVal)
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
                if (filterVal == -1||filterVal==null) {
                posts = await PostManager.FindAllDescenderAsync(filter);
                }
                else
                {
                    
                posts = await PostManager.FindPostsByTypeAsync(filter, filterVal);

                }
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
        public async Task<ActionResult> _suggestedPost(string postId,int? pageNum)
        {
            Post post = await PostManager.FindByIdAsync(postId);
            List<Tag> tagsList = post.Tags;
            BaseFilter filter = new BaseFilter { CurrentPage = pageNum.Value };
            var result = await PostManager.FindPostByTagsAsync(filter, tagsList);
            var list = new List<PostViewModel>();
            foreach (var item in result)
            {
                var postView = new PostViewModel
                {
                    Title = (string)item["Title"],
                    AvartarLink = await UserManager.GetAvatarByIdAsync(item["UserId"].ToString()),
                    Description = item["Description"].ToString(),
                    Id = item["_id"].ToString()
                };
                if (item.Contains("Illustrations"))
                {
                    var illustrations = new List<Illustration>();
                    foreach (var il in item["Illustrations"].AsBsonArray)
                    {
                        var illustration = new Illustration();
                        if (il["PhotoLink"] != null) illustration.PhotoLink = il["PhotoLink"].ToString();
                        //todo Description                        
                        illustrations.Add(illustration);
                    }
                    postView.Illustrations = illustrations;
                }
                postView.Status = item["Status"].AsBoolean;
                postView.UserId = item["UserId"].ToString();
                postView.TimeInterval = Utilities.GetTimeInterval(new DateTimeOffset
                    (item["CreatedDate"].AsBsonArray[0].ToInt64(),
                    Utilities.ConvertTimeZoneOffSetToTimeSpan(
                    item["CreatedDate"].AsBsonArray[1].ToInt32())));
                postView.UserName = await UserManager.GetUserNameByIdAsync(item["UserId"].ToString());
                list.Add(postView);
            }
            return PartialView(list);
        }
        [AllowAnonymous]
        public async Task<ActionResult> ShowPostDetailPage(string Id)
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
        public async Task<ActionResult> _ShowPostDetailModal(string postId)
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
     

        public JsonResult GetCommentInfo(string commentid)
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
           
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(postId);
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
            CloudBlobContainer blobContainer= _blobClient.GetContainerReference(pPostView.Id);
            await blobContainer.CreateIfNotExistsAsync();
            blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
            List<Illustration> illList=new List<Illustration>();
         
            _illustrationList = (HttpFileCollectionBase)Session["Illustrations"];
            if (_illustrationList?.Count > 0) { 
                for (int i = 0; i < _illustrationList.Count; i++)
            {

                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(_illustrationList[i].FileName));
                await blob.UploadFromStreamAsync(_illustrationList[i].InputStream);

            }
                
            }
            var blobList = blobContainer.ListBlobs();
            
            foreach (var blob in blobList)
            {
                Illustration newIll = new Illustration(blob.Uri.ToString());
                illList.Add(newIll);
            }
            if (illList.Count>0)
            {
                pPostView.Illustrations = illList;
            }
            Post post = new Post(pPostView);           
            await PostManager.UpdateAsync(post);
            return RedirectToAction("ShowPostDetailPage", "Newsfeed", new { pPostView.Id });
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
            var illList = await PostManager.AzureUploadAsync(_illustrationList, pPostView.Id);
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
            await PostManager.DeleteByIdAsync(postId);
            CloudBlobContainer blobContainer = _blobClient.GetContainerReference(postId);
            await blobContainer.DeleteIfExistsAsync();
            return RedirectToAction("Index", "Newsfeed");
        }
        public async Task DeleteImages(string name, string id)
        {
           
           
            try
            {
                await PostManager.AzureDeleteAsync(name, id);


            }

            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
            }
        }      
    }
}