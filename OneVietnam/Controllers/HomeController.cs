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
            var userslist = await UserManager.TextSearchByUserName(id);             

            List<UserViewModel> listview = userslist.Select(user => new UserViewModel(user)).ToList();
            SearchResultModel s = new SearchResultModel
            {
                Count = listview.Count,
                UserList = listview
            };
            return Json(s, JsonRequestBehavior.AllowGet);
        }
    }
}