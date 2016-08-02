using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;
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
            //todo

            // Hien top 5 users va 1 page row cac bai post, sau do thuc hien infiniti scroll giong trang timeline
            var usersBaseFilter = new BaseFilter
            {                
                Limit = Constants.LimitedNumberDisplayUsers
            };
            ApplicationUser postUser = await UserManager.FindByIdAsync("5786665d59b07a1e205bfd48");
            UserViewModel user=new UserViewModel(postUser);
            //            var userResult = await UserManager.TextSearchUsers(query, usersBaseFilter);
            //            var postsBaseFilter = new BaseFilter();
            //            var postResult = await PostManager.FullTextSearch(query, postsBaseFilter);
            //            List<PostViewModel> postViewModels = new List<PostViewModel>();
            //            foreach (var post in postResult)
            //            {
            //                postViewModels.Add(new PostViewModel(post));
            //            }
            //            return View(postViewModels);

            return View(user);
        }
        //todo
        //public async Task<ActionResult> UsersResult(string query)
        //{
        //    var baseFilter = new BaseFilter();
        //    var users = await UserManager.TextSearchUsers(query, baseFilter).ConfigureAwait(false);
        //    return View();
        //}
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
                ItemsPerPage = Constants.ResultMaximumNumber
            };

            var result = await PostManager.FullTextSearch(query, filter);
            var list = new List<SearchResultItem>();
            foreach (var item in result)
            {
                var searchItem = new SearchResultItem
                {
                    Url = Url.Action("ShowPostDetail", "Newsfeed", new { postId = item["_id"].ToString() })
                };
                //searchItem.Description = item["Description"].AsString.Substring(0,Math.Min(200, item["Description"].AsString.Length));
                if (item["Description"].AsString.Length > Constants.DescriptionMaxLength)
                {
                    searchItem.Description = item["Description"].AsString.Substring(0, Constants.DescriptionMaxLength) + "...";
                }
                else
                {
                    searchItem.Description = item["Description"].AsString;
                }
                if (item["Title"].AsString.Length > Constants.TitleMaxLength)
                {
                    searchItem.Title = item["Title"].AsString.Substring(0, Constants.TitleMaxLength) + "...";
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
                Url = Url.Action("Index","Timeline",new {userId=user.Id})
            }).ToList();
            var searchResult = new SearchResultModel
            {
                Count = list.Count,
                Result = list
            };
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SearchUserMultipleQuery()
        {
            string userName = "";
            DateTimeOffset? createdDateFrom = null;
            DateTimeOffset? createdDateTo = null;
            string role = "";
            bool? isConnection = null;

            if (Request.Form.Count > 0)
            {
                userName = Request.Form["txtSearchUserName"];
                string dateFrom = Request.Form["dtCreatedDateFrom"];
                if (!string.IsNullOrWhiteSpace(dateFrom))
                {
                    createdDateFrom = Convert.ToDateTime(dateFrom).ToUniversalTime();
                }
                string dateTo = Request.Form["dtCreatedDateTo"];
                if (!string.IsNullOrWhiteSpace(dateTo))
                {                    
                    createdDateTo = Convert.ToDateTime(dateTo).AddHours(24).ToUniversalTime();
                }
                role = Request.Form["txtSearchUserRole"];
                var connection = Request.Form["chkIsOnline"];
                if (!string.IsNullOrWhiteSpace(connection) && string.Equals(connection, "on"))
                {
                    isConnection = true;
                }
            }
            var users = await UserManager.TextSearchMultipleQuery(userName, createdDateFrom, createdDateTo, role, isConnection);
            List<UserManagementViewModel> userViews = new List<UserManagementViewModel>();
            if (users != null && users.Count > 0)
            {
                foreach (var item in users)
                {
                    UserManagementViewModel model = new UserManagementViewModel(item);
                    userViews.Add(model);
                }

            }
            return PartialView("../Administration/_UsersManagementPanel", userViews);
        }

        [HttpPost]
        public async Task<ActionResult> SearchPostMultipleQuery()
        {
            string postTitle = "";
            DateTimeOffset? createdDateFrom = null;
            DateTimeOffset? createdDateTo = null;            
            bool? postStatus = null;            
            if (Request.Form.Count > 0)
            {
                postTitle = Request.Form["txtSearchPostTitle"];                
                string dateFrom = Request.Form["dtPostCreatedDateFrom"];
                if (!string.IsNullOrWhiteSpace(dateFrom))
                {
                    createdDateFrom = Convert.ToDateTime(dateFrom).ToUniversalTime();
                }
                string dateTo = Request.Form["dtPostCreatedDateTo"];
                if (!string.IsNullOrWhiteSpace(dateTo))
                {
                    createdDateTo = Convert.ToDateTime(dateTo).AddHours(24).ToUniversalTime();
                }                
                var status = Request.Form["rdStatus"];
                if (!string.IsNullOrWhiteSpace(status) && !string.Equals(status, "all"))
                {
                    postStatus = Convert.ToBoolean(status);
                }
            }
            var posts = await PostManager.SearchPostMultipleQuery(postTitle, createdDateFrom, createdDateTo, postStatus);
            List<AdminPostViewModel> postViews = new List<AdminPostViewModel>();
            if (posts != null && posts.Count > 0)
            {
                foreach (var item in posts)
                {
                    AdminPostViewModel model = new AdminPostViewModel(item);
                    postViews.Add(model);
                }

            }
            return PartialView("../Administration/_PostsManagementPanel", postViews);
        }


    }
}