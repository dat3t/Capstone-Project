
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class TimelineController : Controller
    {
        public TimelineController()
        {
        }

        public TimelineController(ApplicationUserManager userManager)
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

        private PostManager _postManager;
        public PostManager PostManager
        {
            get
            {
                return _postManager ?? HttpContext.GetOwinContext().Get<PostManager>();
            }
            private set { _postManager = value; }
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

        public List<Tag> TagList
        {
            get
            {
                var tags = TagManager.GetTagsAsync();
                return tags?.Result;
            }
        }
        public List<Icon> IconList
        {
            get
            {
                var icons = IconManager.GetIconPostAsync();
                return icons;
            }
        }

        //ThamDTH 
        public async Task<ActionResult> Timeline(string userId)
        {            
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                List<Post> posts = await PostManager.FindByUserId(userId);
                if (IconList != null)
                {
                    ViewData["PostTypes"] = IconList;
                }
                TimelineViewModel timeLine = new TimelineViewModel(user, posts);
                return View(timeLine);
            }
            return View();
        }

        [HttpGet]
        [System.Web.Mvc.Authorize]
        public async Task<PartialViewResult> EditProfile()
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            UserProfileViewModel profile = new UserProfileViewModel(user);
            return PartialView("_EditProfile", profile);
        }

        [HttpPost]
        public async Task<PartialViewResult> EditProfile(int gender, string phone, string address)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {                
                user.Gender = gender;
                if (user.Location != null)
                {
                    user.Location.Address = address;
                }
                else
                {
                    user.Location = new Location {Address = address};
                }

                user.PhoneNumber = phone;
                await UserManager.UpdateAsync(user);
                await SignInAsync(user, isPersistent: false);
                UserProfileViewModel profile = new UserProfileViewModel(user);
                return PartialView("_ShowProfile", profile);
            }
            return null;
        }

        [HttpGet]
        public async Task<PartialViewResult> ShowProfile(string userId)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            UserProfileViewModel profile = new UserProfileViewModel(user);
            return PartialView("_ShowProfile", profile);
        }

        [HttpGet]       
        public async Task<PartialViewResult> ShowTwoFactorAuthen(string userId)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            TwoFacterViewModel setting = new TwoFacterViewModel(user);
            return PartialView("_ShowTwoFactorAuthen", setting);
        }

        [HttpPost]
        public async Task<PartialViewResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                TwoFacterViewModel setting = new TwoFacterViewModel(user);
                return PartialView("_ShowTwoFactorAuthen", setting);
            }
            return null;
        }
        
        [HttpPost]        
        public async Task<PartialViewResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                TwoFacterViewModel setting = new TwoFacterViewModel(user);
                return PartialView("_ShowTwoFactorAuthen", setting);
            }
            return null;
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {            
            return PartialView("_ChangePassword", new ChangePasswordViewModel());
        }

        [HttpPost]               
        public async Task<ActionResult> ChangePassword(string oldPass, string newPass, string confirmPass)
        {            
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), oldPass, newPass);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return null;
            }            
            return PartialView("_ChangePassword", new ChangePasswordViewModel(oldPass, newPass, confirmPass));
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }        

    }
}