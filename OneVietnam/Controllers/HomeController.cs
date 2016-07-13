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
    public class HomeController : Controller
    {
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Search(string id)
        {
            var result = await PostManager.FullTextSearch(id);
            var list = (from post in result
                where post.DeletedFlag == false
                select new SearchResultItem
                {
                    Description = post.Description, Title = post.Title, Url= Url.Action("ShowPostDetail","Post",new { postId = post.Id})
                }).ToList();            
            var searchResult = new SearchResultModel
            {
                Count = result.Count,
                Result = list
            };
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }
    }
}