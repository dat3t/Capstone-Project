using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.Common;
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
            List<PostViewModel> postViewModels = new List<PostViewModel>();
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
            //var result = await PostManager.FullTextSearch(query);
            var filter = new BaseFilter
            {
                CurrentPage = 1,
                ItemsPerPage = Common.Constants.ResultMaximumNumber
            };

            var result = await PostManager.FullTextSearch(query, filter);
            var list = new List<SearchResultItem>();
            foreach (var item in result)
            {
                var searchItem = new SearchResultItem
                {
                    Url = Url.Action("ShowPostDetail", "Post", new { postId = item["_id"].ToString() })
                };
                //searchItem.Description = item["Description"].AsString.Substring(0,Math.Min(200, item["Description"].AsString.Length));
                if (item["Description"].AsString.Length > Common.Constants.DescriptionMaxLength)
                {
                    searchItem.Description = item["Description"].AsString.Substring(0, Common.Constants.DescriptionMaxLength) + "...";
                }
                else
                {
                    searchItem.Description = item["Description"].AsString;
                }
                if (item["Title"].AsString.Length > Common.Constants.TitleMaxLength)
                {
                    searchItem.Title = item["Title"].AsString.Substring(0, Common.Constants.TitleMaxLength) + "...";
                }
                else
                {
                    searchItem.Title = item["Title"].AsString;
                }

                //searchItem.Title = item["Title"].AsString.Substring(0, Math.Min(100, item["Title"].AsString.Length));         
                list.Add(searchItem);
            }
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
                Description = user.Email,
                Title = user.UserName,
                Url = ""
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