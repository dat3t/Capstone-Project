
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
                var tags = TagManager.FindAllAsync();
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
        public async Task<PartialViewResult> EditProfile()
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            UserProfileViewModel profile = new UserProfileViewModel(user);
            return PartialView("_EditProfile", profile);
        }        

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditProfile(UserProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditProfile", profile);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                if (profile.UserName != null)
                {
                    user.UserName = profile.UserName;
                }
                user.Gender = profile.Gender;
                user.Email = profile.Email;
                user.Location.Address = profile.Location;
                if (profile.DateOfBirth != null)
                {
                    user.DateOfBirth = profile.DateOfBirth;
                }
                else
                {
                    user.DateOfBirth = null;
                }
                
                if(profile.PhoneNumber != null)
                {
                    user.PhoneNumber = profile.PhoneNumber;
                }                
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);                                        
                }
                AddErrors(result);                
            }
            return PartialView("_EditProfile", profile);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        public async Task ChangeTwoFactorAuthentication(string value)
        {
            if (value == "Bật")
            {
                await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            }
            else
            {
                await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);

            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                TwoFacterViewModel setting = new TwoFacterViewModel(user);                
            }            
        }
               
        [HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult ChangePassword()
        {            
            return PartialView("_ChangePassword", new ChangePasswordViewModel());
        }       

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ChangePassword", model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return null;
            }
            AddErrors(result);            
            return PartialView("_ChangePassword", model);
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}