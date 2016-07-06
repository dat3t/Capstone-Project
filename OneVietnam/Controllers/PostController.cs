
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

        public ActionResult CreatePost()
        {
            var tagList = TagManager.GetTagsAsync();
            if (tagList != null)
            {
                ViewData["TagList"] = tagList.Result;
            }                                   
            var icons = IconManager.GetIconPostAsync();
            if (icons != null)
            {
                ViewData["PostTypes"] = icons;
            }
            
            return View();
        }        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(CreatePostViewModel p, List<Tag> pTags)
        {
            ViewData.Clear();
            p.UserName = User.Identity.Name;
            var tagList = GetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
            if (tagList != null)
            {
                p.Tags = tagList;
            }

            if (illList != null)
            {
                p.Illustrations = illList;
            }
            var post = new Post(p);
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
        [HttpPost]
        public ActionResult ShowCreatedPost(ShowPostViewModel pPostView)
        {
            return RedirectToAction("EditPost");
        }

        public ActionResult EditPost()
        {
            var tagList = TagManager.GetTagsAsync();
            if (tagList != null)
            {
                ViewData["TagList"] = tagList.Result;
            }
            var icons = IconManager.GetIconPostAsync();
            if (icons != null)
            {
                ViewData["PostTypes"] = icons;
            }
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

        public List<Tag> GetAddedTags(HttpRequestBase pRequestBase, TagManager pTagManager, string pFormId)
        {
            if (pRequestBase.Form.Count > 0)
            {
                var addedTagValueList = pRequestBase.Form[pFormId];
                if (!string.IsNullOrEmpty(addedTagValueList.ToString()))
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
                if (!string.IsNullOrEmpty(addedImgSrcList) && !string.IsNullOrEmpty(addedImgDesList) )
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