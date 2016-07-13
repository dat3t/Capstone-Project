
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
        public List<Tag> TagList
        {
            get
            {
                var tags = TagManager.GetTagsAsync();
                return tags?.Result;
            }                    
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
            p.UserName = User.Identity.Name;            
            var tagList = AddAndGetAddedTags(Request, TagManager, "TagsInput");
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
                PublishDate = System.DateTime.Now,
                UserId = User.Identity.GetUserId()
            };

            await PostManager.CreatePostAsync(post);
            CreatedPost = true;
            PostView = new PostViewModel(post);
            return RedirectToAction("ShowPostDetail", "Post", new { postId = post.Id });
        }

        public async Task<ActionResult> ShowPost()
        {
            List<Post> list = await PostManager.FindByUserId(User.Identity.GetUserId());
            List<PostViewModel> pViewList = list.Select(post => new PostViewModel(post)).ToList();
            return View(pViewList);
        }

        public void _ShowPostDetail(string postId)
        {            
            //if (IconList != null)
            //{
            //    ViewData["PostTypes"] = IconList;
            //}
            //Post post = UserManager.GetPostByIdAsync(postId);
            //PostViewModel showPost = new PostViewModel(post);
            //ViewData["PostView"] = showPost;
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
            
            Post post = await PostManager.FindById(postId);
            ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
            if (postUser != null)
            {
                ViewData["PostUser"] = postUser;
            }
            PostViewModel showPost = new PostViewModel(post);            
            return View(showPost);
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
            Post post = await PostManager.FindById(postId);
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
            var tagList = AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
            if (tagList != null)
            {
                pPostView.Tags = tagList;
            }

            if (illList != null)
            {
                pPostView.Illustrations = illList;
            }
            Post post = new Post(pPostView, strPostId);           
            await PostManager.UpdatePostAsync(post);
            return RedirectToAction("ShowPostDetail", "Post", new { postId = strPostId });
        }

        public async Task<ActionResult> DeletePost(string postId)
        {
            Post post = await PostManager.FindById(postId);
            post.DeletedFlag = true;
            //await PostManager.DeleteByIdAsync(postId);            
            await PostManager.UpdatePostAsync(post);
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

        public List<Tag> AddAndGetAddedTags(HttpRequestBase pRequestBase, TagManager pTagManager, string pFormId)
        {
            if (pRequestBase.Form.Count > 0)
            {
                var addedTagValueList = pRequestBase.Form[pFormId];
                if (!string.IsNullOrEmpty(addedTagValueList))
                {
                    List<Tag> newList = new List<Tag>();
                    var tagsInDb = pTagManager.GetTagsValueAsync();                    
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
                            pTagManager.CreateAsync(newTag);
                            numberTags = numberTags + 1;
                            newList.Add(newTag);
                        }
                        else
                        {
                            var existTag = pTagManager.FindTagByValueAsync(tag);
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