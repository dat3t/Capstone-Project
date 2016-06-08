using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneVietnam.Controllers
{
    public class MapController : Controller
    {
        public static LocationViewModel LocationView;
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        //DEMO
        public ActionResult AddLocation()
        {
            return View();
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddLocation(LocationViewModel l)
        {
            Location location = new Location(l);
            await UserManager.AddLocationAsync(l.userid,location);
            //createdPost = true;
            //PostView = new ShowPostViewModel(post);
            return RedirectToAction("Index", "Home");
        }

        //public ActionResult ShowLocation()
        //{
        //    return View(LocationView);
        //}

        public async Task<ActionResult> ShowLocation()
        {
            var userslist = await UserManager.AllUsersAsync();
            List<Location> list = await UserManager.GetPostsAsync(User.Identity.GetUserId());
            List<LocationViewModel> locationViewList = list.Select(post => new ShowPostViewModel(post)).ToList();
            return View(pViewList);
        }

    }
}