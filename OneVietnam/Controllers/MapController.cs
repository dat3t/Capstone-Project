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

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> AddLocation(AddLocationViewModel l)
        //{
        //    Location location = new Location(l);
        //    await UserManager.AddLocationAsync(l.userid,location);
        //    return RedirectToAction("Index", "Home");
        //}

        public async Task<ActionResult> ShowMap()
        {
            var userslist = await UserManager.AllUsersAsync();
            var list = userslist.Select(user => new AddLocationViewModel
            {
                X = user.Location.XCoordinate, Y = user.Location.YCoordinate, UserId = user.Id, Gender = user.Gender
            }).ToList();
            var postlist = await PostManager.FindAllPostsAsync();
            list.AddRange(postlist.Select(p => new AddLocationViewModel
            {
                UserId = p.UserId, X = p.PostLocation.XCoordinate, Y = p.PostLocation.YCoordinate, PostId = p.Id, PostType = p.PostType
            }));

            return View(list);
        }

    }
}