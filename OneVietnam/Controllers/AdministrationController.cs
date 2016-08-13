using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;
using OneVietnam.BLL;
using OneVietnam.Common;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.Mod)]
    public class AdministrationController : Controller
    {
        public AdministrationController() { }

        public AdministrationController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
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
        private PostManager _postManager;
        public PostManager PostManager
        {
            get
            {
                return _postManager ?? HttpContext.GetOwinContext().Get<PostManager>();
            }
            private set { _postManager = value; }
        }

        private ReportManager _reportManager;
        public ReportManager ReportManager
        {
            get
            {
                return _reportManager ?? HttpContext.GetOwinContext().Get<ReportManager>();
            }
            private set { _reportManager = value; }
        }


        public async Task<ActionResult> Index()
        {

            var users = await UserManager.TextSearchMultipleQuery("", DateTimeOffset.Now.Date.AddDays(-7).ToUniversalTime(), DateTimeOffset.Now.Date.AddDays(1).ToUniversalTime(), "", null);
            var roles = await RoleManager.AllRolesAsync();
            var posts = await PostManager.SearchPostMultipleQuery("", DateTimeOffset.Now.Date.AddDays(-7).ToUniversalTime(), DateTimeOffset.Now.Date.AddDays(1).ToUniversalTime(), null);
            var reports = await ReportManager.FindAllAsync();
            List<ReportViewModel> reportViewList = new List<ReportViewModel>();
            foreach (var report in reports)
            {
                ReportViewModel reportView = new ReportViewModel(report);
                if (!string.IsNullOrWhiteSpace(report.HandlerId))
                {
                    var handlerUser = await UserManager.FindByIdAsync(report.HandlerId);
                    if (handlerUser != null)
                    {
                        reportView.HandlerName = handlerUser.UserName;
                    }
                }
                if (!string.IsNullOrWhiteSpace(report.PostId))
                {
                    var reportedPost = await PostManager.FindByIdAsync(report.PostId);
                    if (reportedPost != null)
                    {
                        reportView.PostTile = reportedPost.Title;
                    }
                }
                var reportedUser = await UserManager.FindByIdAsync(report.UserId);
                if (!string.IsNullOrWhiteSpace(reportedUser.UserName))
                {
                    reportView.UserName = reportedUser.UserName;
                }

                reportViewList.Add(reportView);

            }

            AdministrationViewModel administrationView = new AdministrationViewModel(users, roles, posts, reportViewList);
            return View(administrationView);
        }

        [HttpPost]
        [Authorize(Roles = CustomRoles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(RoleViewModel roleViewModel)
        {

            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("",
                        roleresult.Errors.First().Contains("is already taken")
                            ? "Quyền này đã tồn tại trong cơ sở dữ liệu."
                            : roleresult.Errors.First());
                }

            }
            var roleList = await RoleManager.AllRolesAsync();

            List<RoleViewModel> roles = new List<RoleViewModel>();
            if (roleList != null && roleList.Count > 0)
            {
                foreach (var oneRole in roleList)
                {
                    RoleViewModel roleView = new RoleViewModel(oneRole);
                    roles.Add(roleView);
                }
            }
            return PartialView("_RoleManagementPanel", roles);
        }

        [HttpPost]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> RemoveRole(string roleId)
        {
            //Remove role in roles document
            var role = await RoleManager.FindByIdAsync(roleId);
            var roleresult = await RoleManager.DeleteAsync(role);
            if (!roleresult.Succeeded)
            {
                ModelState.AddModelError("", roleresult.Errors.First());
            }

            //Remove role if user has this role
            var users = await UserManager.FindUsersByRoleAsync(role);
            foreach (var user in users)
            {
                user.Roles.Remove(role.Name);
                var userResult = await UserManager.UpdateAsync(user);
                if (!userResult.Succeeded)
                {
                    ModelState.AddModelError("", userResult.Errors.First());
                }
            }

            //Return new role list after remove this role
            var roleList = await RoleManager.AllRolesAsync();
            List<RoleViewModel> roles = new List<RoleViewModel>();
            if (roleList != null && roleList.Count > 0)
            {
                foreach (var oneRole in roleList)
                {
                    RoleViewModel roleView = new RoleViewModel(oneRole);
                    roles.Add(roleView);
                }
            }
            return PartialView("_RoleManagementPanel", roles);
        }

        [HttpGet]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> GetOtherRoles(string userId)
        {
            var roleList = await RoleManager.AllRolesAsync();
            var user = await UserManager.FindByIdAsync(userId);

            List<IdentityRole> roles = new List<IdentityRole>();
            if (roleList != null && roleList.Count > 0)
            {
                foreach (var role in roleList)
                {
                    if (user?.Roles != null && user.Roles.Count > 0)
                    {
                        if (!user.Roles.Contains(role.Name))
                        {
                            roles.Add(role);
                        }
                    }
                    else
                    {
                        roles.Add(role);
                    }
                }
            }
            ViewData["OtherRole"] = roles;
            UserManagementViewModel userView = new UserManagementViewModel(user);
            return PartialView("_AddUserRole", userView);
        }

        [HttpPost]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> RemoveUserRole(string userId, string role)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                if (user?.Roles != null && user.Roles.Count > 0)
                {
                    user.Roles.Remove(role);
                }
                var result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                }
            }
            UserManagementViewModel userView = new UserManagementViewModel(user);
            return PartialView("_UserRoles", userView);
        }

        [HttpPost]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> AddUserRole(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (user.Roles == null)
                    {
                        user.Roles = new List<string>();
                    }
                    if (Request.Form.Count > 0)
                    {
                        string roleAdded = Request.Form["txtUserRole"];
                        if (string.IsNullOrEmpty(roleAdded))
                        {
                            ModelState.AddModelError("", "Chưa chọn quyền.");
                        }
                        else
                        {
                            user.Roles.Add(roleAdded);
                            var result = await UserManager.UpdateAsync(user);
                            if (!result.Succeeded)
                            {
                                ModelState.AddModelError("", result.Errors.First());
                            }
                        }
                    }
                }

            }
            UserManagementViewModel userView = new UserManagementViewModel();
            if (user != null)
            {
                userView = new UserManagementViewModel(user);
            }

            return PartialView("_UserRoles", userView);
        }

        [HttpPost]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<ActionResult> ChangeLockedStatus(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.LockedFlag = !user.LockedFlag;
                    var result = await UserManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                    }
                }

            }
            UserManagementViewModel userView = new UserManagementViewModel();
            if (user != null)
            {
                userView = new UserManagementViewModel(user);
            }

            return PartialView("_UserLockedStatus", userView);
        }

        [HttpPost]
        public void GetUploadImage()
        {
            Session["IllustrationList"] = Request.Files;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdminPost(CreateAdminPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adress = ((ClaimsIdentity)User.Identity).FindFirst("Adress").Value;
                var xCoordinate = Convert.ToDouble(((ClaimsIdentity)User.Identity).FindFirst("XCoordinate").Value);
                var yCoordinate = Convert.ToDouble(((ClaimsIdentity)User.Identity).FindFirst("YCoordinate").Value);
                var location = new Location(xCoordinate, yCoordinate, adress);
                Post post = new Post(model) { UserId = User.Identity.GetUserId(), PostLocation = location };
                HttpFileCollectionBase files = (HttpFileCollectionBase)Session["IllustrationList"];
                var illList = await PostManager.GetIllustration(files, post.Id);
                Session["Illustrations"] = null;
                if (illList != null)
                {
                    post.Illustrations = illList;
                }
                try
                {
                    await PostManager.CreateAsync(post);
                    return PartialView("../Administration/_CreateAdminPost", new CreateAdminPostViewModel(post));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return PartialView("../Administration/_CreateAdminPost", model);
                }
            }

            return PartialView("../Administration/_CreateAdminPost", new CreateAdminPostViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> ChangeReportStatus(string reportAction, string reportId)
        {
            var report = await ReportManager.FindByIdAsync(reportId);
            try
            {
                report.Status = reportAction;
                report.HandlerId = User.Identity.GetUserId();
                if (string.Equals(reportAction, ReportStatus.Closed.ToString()) || string.Equals(reportAction, ReportStatus.Canceled.ToString()))
                {
                    report.CloseDate = DateTimeOffset.UtcNow;
                }
                await ReportManager.UpdateAsync(report);
                if (string.Equals(reportAction, ReportStatus.Closed.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(report.PostId))
                    {
                        var post = await PostManager.FindByIdAsync(report.PostId);
                        post.LockedFlag = true;
                        await PostManager.UpdateAsync(post);
                    }
                    else
                    {
                        var user = await UserManager.FindByIdAsync(report.UserId);
                        user.LockedFlag = true;
                        await UserManager.UpdateAsync(user);
                    }
                }

                ReportViewModel viewModal = new ReportViewModel(report);
                return PartialView("../Administration/_ShowReportStatus", viewModal);
            }
            catch
            {
                ReportViewModel viewModal = new ReportViewModel(report);
                return PartialView("../Administration/_ShowReportStatus", viewModal);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateIcon(CreateIconViewModel model)
        {
            var icon = new Icon(model);
            await IconManager.CreateAsync(icon);
            return RedirectToAction("Index", "Administration");
        }
    }
}