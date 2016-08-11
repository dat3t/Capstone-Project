using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        // GET: Search
        [AllowAnonymous]
        public async Task<ActionResult> Index(string query,int? pageNum)
        {
            //todo

          
            pageNum = pageNum ?? 1;
            ViewBag.IsEndOfRecords = false;

            BaseFilter filter;
            List<PostViewModel> listPost = new List<PostViewModel>();
            if (Request.IsAjaxRequest())
            {
                filter = new BaseFilter { CurrentPage = pageNum.Value };
                listPost = await PostSearch(query,filter);

                if (listPost.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
               
                //ViewBag.IsEndOfRecords = (posts.Any()) && ((pageNum.Value * RecordsPerPage) >= posts.Last().Key);
                return PartialView("_PostRow", listPost);
            }
           
      
            filter = new BaseFilter { CurrentPage = pageNum.Value };
            listPost = await PostSearch(query, filter);
            //posts = await PostManager.FullTextSearch(qu)
            if (listPost.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
        
            ViewBag.Posts = listPost;
            ViewBag.Query = query;
            return View();
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
        [AllowAnonymous]
        public async Task<ActionResult> _userResult(string query,int? pageNum)
        {
            
            ViewBag.IsEndOfRecords = false;

            BaseFilter filter;
            List<UserViewModel> list = new List<UserViewModel>();
            List<ApplicationUser> users = new List<ApplicationUser>();
          
                filter = new BaseFilter { CurrentPage = pageNum.Value };
                users = await UserManager.TextSearchUsers(filter, query);

                if (users.Count < filter.ItemsPerPage) ViewBag.IsEndOfRecords = true;
                list = users.Select(p => new UserViewModel(p)).ToList();
                //ViewBag.IsEndOfRecords = (posts.Any()) && ((pageNum.Value * RecordsPerPage) >= posts.Last().Key);
                return PartialView("_userRow", list);
            

           
        }
        [AllowAnonymous]
        public async Task<List<PostViewModel>> PostSearch(string query, BaseFilter baseFilter)
        {
            var result = await PostManager.FullTextSearch(query, baseFilter);
            var list = new List<PostViewModel>();
            foreach (var item in result)
            {
                var postView = new PostViewModel
                {
                    Title = (string) item["Title"],
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
                postView.TimeInterval = Utilities.GetTimeInterval(new DateTimeOffset
                    (item["CreatedDate"].AsBsonArray[0].ToInt64(), 
                    Utilities.ConvertTimeZoneOffSetToTimeSpan(
                    item["CreatedDate"].AsBsonArray[1].ToInt32())));                
                postView.UserName = await UserManager.GetUserNameByIdAsync(item["UserId"].ToString());
                list.Add(postView);                
            }
            return list;
        }
        [AllowAnonymous]
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
                    Url = Url.Action("ShowPost", "Newsfeed", new { Id = item["_id"].ToString() })
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
        [AllowAnonymous]
        public async Task<ActionResult> UsersSearch(string query)
        {
            var baseFilter = new BaseFilter {Limit = Constants.LimitedNumberDisplayUsers};
            var result = await UserManager.TextSearchUsers(baseFilter,query);

            var list = result.Select(user => new SearchResultItem()
            {
                Description = user.Email,
                Title = user.UserName,
                Url = Url.Action("Index","Timeline",new {Id=user.Id})
            }).ToList();
            var searchResult = new SearchResultModel
            {
                Count = list.Count,
                Result = list
            };
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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