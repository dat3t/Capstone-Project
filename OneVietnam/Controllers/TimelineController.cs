
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class TimelineController : Controller
    {
        static CloudBlobClient blobClient;
        const string blobContainerName = "avatar";
        static CloudBlobContainer blobContainer;
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

        public async Task<ActionResult> Index(string userId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a blob client for interacting with the blob service.
            blobClient = storageAccount.CreateCloudBlobClient();

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
                user.Location = profile.Location;
                if (profile.DateOfBirth != null)
                {
                    user.DateOfBirth = profile.DateOfBirth;
                }
                else
                {
                    user.DateOfBirth = null;
                }

                if (profile.PhoneNumber != null)
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

        [HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult SetPassword()
        {
            return PartialView("_SetPassword", new SetPasswordViewModel());
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        result = await UserManager.SetEmailConfirmed(user);
                        if (result.Succeeded)
                        {
                            await SignInAsync(user, isPersistent: false);
                            return null;
                        }
                        AddErrors(result);
                    }
                    return PartialView("_SetPassword", model);
                }
                AddErrors(result);
            }
            return PartialView("_SetPassword", model);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        public async Task<ActionResult> ChangeAvatar()
        {
            blobContainer = blobClient.GetContainerReference("avatar");
            await blobContainer.CreateIfNotExistsAsync();

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
            // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
            // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            HttpFileCollectionBase file = Request.Files;

            if (file != null && file.Count > 0)
            {
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(User.Identity.GetUserId());
                await blob.DeleteIfExistsAsync();
                await blob.UploadFromStreamAsync(file[0].InputStream);
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    user.Avatar = blob.Uri.ToString();
                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    AddErrors(result);
                }
            }
            return RedirectToAction("Index", "Timeline", new { userId = User.Identity.GetUserId() });

        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        public async Task<ActionResult> ChangeCover()
        {
            blobContainer = blobClient.GetContainerReference("cover");
            await blobContainer.CreateIfNotExistsAsync();

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
            // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
            // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            HttpFileCollectionBase file = Request.Files;

            if (file != null && file.Count > 0)
            {
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(User.Identity.GetUserId());
                await blob.DeleteIfExistsAsync();
                await blob.UploadFromStreamAsync(file[0].InputStream);
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    user.Cover = blob.Uri.ToString();
                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    AddErrors(result);
                }
            }
            return RedirectToAction("Index", "Timeline", new { userId = User.Identity.GetUserId() });

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