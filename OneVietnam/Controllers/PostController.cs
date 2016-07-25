
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
        static CloudBlobClient blobClient;
        const string blobContainerName = "webappstoragedotnet-imagecontainer";
        static CloudBlobContainer blobContainer;
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
        [System.Web.Mvc.Authorize]
        public ActionResult CreatePost()
        {            
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }                                               
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }
            return View();
        }        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(CreatePostViewModel p)
        {
            ViewData.Clear();                  
            var tagList = await AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
            if (tagList != null)
            {
                p.Tags = tagList;
            }

            if (illList != null)
            {
                p.Illustrations = illList;
            }
            var post = new Post(p)
            {
                CreatedDate = System.DateTime.Now,
                UserId = User.Identity.GetUserId()
            };

            await PostManager.CreateAsync(post);
            CreatedPost = true;
            PostView = new PostViewModel(post);
            return RedirectToAction("NewFeeds", "Post", new { postId = post.Id });
        }
        //public async Task<ActionResult> TimeLine()
        //{
        //    return RedirectToAction("GetPosts");
        //}

        public const int RecordsPerPage = 60;

        public async Task<ActionResult> Newfeeds(int? pageNum)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a blob client for interacting with the blob service.
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blobContainerName);
            await blobContainer.CreateIfNotExistsAsync();

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
            // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
            // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            if (Request.Form["createpost"] != null)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    int fileCount = files.Count;

                    if (fileCount > 0)
                    {
                        for (int i = 0; i < fileCount; i++)
                        {
                            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(files[i].FileName));



                            blob.UploadFromStream(files[i].InputStream);

                            //await blob.UploadFromFileAsync(path, FileMode.Open);
                        }
                    }



                }
                catch (Exception ex)
                {

                    ViewData["message"] = ex.Message;
                    ViewData["trace"] = ex.StackTrace;
                    return View("Error");
                }
            }
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
        /// <returns>List<Post></returns>
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
        public async Task<ActionResult> UploadAsync()
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                int fileCount = files.Count;

                if (fileCount > 0)
                {
                    for (int i = 0; i < fileCount; i++)
                    {
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(files[i].FileName));



                        blob.UploadFromStream(files[i].InputStream);

                        //await blob.UploadFromFileAsync(path, FileMode.Open);
                    }
                }



                return RedirectToAction("NewFeeds");
            }
            catch (Exception ex)
            {

                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteImage(string name)
        {
            try
            {
                Uri uri = new Uri(name);
                string filename = Path.GetFileName(uri.LocalPath);

                var blob = blobContainer.GetBlockBlobReference(filename);
                await blob.DeleteIfExistsAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary> 
        /// Task<ActionResult> DeleteAll(string name) 
        /// Documentation References:  
        /// - Delete Blobs: https://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/#delete-blobs
        /// </summary> 
        [HttpPost]
        public async Task<ActionResult> DeleteAll()
        {
            try
            {
                foreach (var blob in blobContainer.ListBlobs())
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                    {
                        await ((CloudBlockBlob)blob).DeleteIfExistsAsync();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        /// <summary> 
        /// string GetRandomBlobName(string filename): Generates a unique random file name to be uploaded  
        /// </summary> 
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
        public async Task<ActionResult> ShowPost()
        {
            //List<Post> list = await PostManager.FindByUserId(User.Identity.GetUserId());
            List<Post> list = await PostManager.FindAllAsync(false);
            List<PostViewModel> pViewList = list.Select(post => new PostViewModel(post)).ToList();
            return View(pViewList);
        }
        //ThamDTH Create
        [HttpGet]
        public async Task<ActionResult> _ShowPostDetail(string postId)
        {            
            if (!string.IsNullOrEmpty(postId))
            {
                Post post = await PostManager.FindByIdAsync(postId);
                if (post != null)
                {
                if (TagList != null)
                {
                    ViewData["TagList"] = TagList;
                }
                if (IconList != null)
                {
                    ViewData["PostTypes"] = IconList;
                }
                    ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
                    if (postUser != null)
                {
                        PostViewModel showPost = new PostViewModel(post, postUser.UserName);
                        return PartialView("../Post/ShowPostDetail", showPost);
                    }
                }
            }                        
            return View();
        }
        //ThamDTH Create
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

        //ThamDTH Create
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

        //ThamDTH Create        
        [HttpPost]
        public async Task ReportPost(string userId, string postId, string description)
        {            
            Post post = await PostManager.FindByIdAsync(postId);
            Report report = new Report(userId, postId, description);            
            post.AddReport(report);
            await PostManager.UpdateAsync(post);
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
            var tagList = await AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
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
            //await PostManager.DeleteByIdAsync(postId);            
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

        public async Task<List<Tag>> AddAndGetAddedTags(HttpRequestBase pRequestBase, TagManager pTagManager, string pFormId)
        {
            if (pRequestBase.Form.Count > 0)
            {
                var addedTagValueList = pRequestBase.Form[pFormId];
                if (!string.IsNullOrEmpty(addedTagValueList))
                {
                    List<Tag> newList = new List<Tag>();
                    var tagsInDb = await pTagManager.GetTagsValueAsync();                    
                    int numberTags = 0;
                    if (tagsInDb != null)
                    {
                        numberTags = tagsInDb.Count;
                    }

                    foreach (var tag in addedTagValueList.Split(','))
                    {
                        if (tagsInDb == null | (tagsInDb != null && !tagsInDb.Contains(tag)))
                        {
                            Tag newTag = new Tag(string.Concat("Tag_", numberTags.ToString()), tag);
                            await pTagManager.CreateAsync(newTag);
                            numberTags = numberTags + 1;
                            newList.Add(newTag);
                        }
                        else
                        {
                            var existTag = await pTagManager.FindTagByValueAsync(tag);
                            newList.Add(existTag[0]);
                        }
                    }
                    return newList;
                }
                return null;
            }
            return null;
        }

        public List<Illustration> GetAddedImage(HttpRequestBase pRequestBase, string pImgSrc, string pImgDes)
        {
            List<Illustration> illList = new List<Illustration>();
            if (pRequestBase.Form.Count > 0)
            {
                var addedImgSrcList = pRequestBase.Form[pImgSrc];
                var addedImgDesList = pRequestBase.Form[pImgDes];
                if (!string.IsNullOrEmpty(addedImgSrcList))
                {
                    var imgSrcs = addedImgSrcList.Split(',');
                    var imgDes = addedImgDesList.Split(',');                                        
                    for (int index = 0; index < imgDes.Length; index++)
                    {

                        Illustration newIll = new Illustration(string.Concat(imgSrcs[2 * index] + ",",imgSrcs[2 * index + 1]), imgDes[index]);
                        illList.Add(newIll);
                    }                    
                }
            }
            return illList;
        }


    }
}