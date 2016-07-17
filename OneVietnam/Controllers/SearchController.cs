using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public async Task<ActionResult> Index(string query)
        {
             var result = await PostManager.FullTextSearch(query);
            List<PostViewModel> postViewModels=new List<PostViewModel>();
            foreach (var post in result)
            {
                postViewModels.Add(new PostViewModel(post));
            }
            return View(postViewModels);
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

        public async Task<ActionResult> Search(string query)
        {
            var result = await PostManager.FullTextSearch(query);
            var list = (from post in result
                        where post.DeletedFlag == false
                        select new SearchResultItem
                        {
                            Description = post.Description,
                            Title = post.Title,
                            Url = Url.Action("ShowPostDetail", "Post", new { postId = post.Id })
                        }).ToList();
            var searchResult = new SearchResultModel
            {
                Count = list.Count,
                Result = list
            };
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }        
        public async Task<ActionResult> UsersSearch(string query)
        {
            var result = await UserManager.TextSearchUsers(query);
            var list = result.Select(user => new SearchResultItem()
            {
                Description = user.Email, Title = user.UserName, Url = ""
            }).ToList();
            var searchResult = new SearchResultModel
            {
                Count = list.Count,
                Result = list
            };
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }
    }
}