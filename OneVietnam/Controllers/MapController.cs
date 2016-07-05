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
        public static AddLocationViewModel LocationView;
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewMap()
        {
            return View();
        }

        public ActionResult CustomInfoWindow()
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
        public async Task<ActionResult> AddLocation(AddLocationViewModel l)
        {
            Location location = new Location(l);
            await UserManager.AddLocationAsync(l.userid,location);
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ShowMap()
        {
            var userslist = await UserManager.AllUsersAsync();
            List<AddLocationViewModel> list = await UserManager.GetInfoForInitMapAsync(userslist);
            /*List<AddLocationViewModel> locationViewList = list.Select(location => new AddLocationViewModel(location)).ToList();*/
            return View(list);
        }

    }
}