using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    
    public class AdministrationController : Controller
    {
        public AdministrationController(){}

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


        private PostManager _postManager;
        public PostManager PostManager
        {
            get
            {
                return _postManager ?? HttpContext.GetOwinContext().Get<PostManager>();
            }
            private set { _postManager = value; }
        }

        public async Task<ActionResult> ShowAdminPanel()
        {
            var users = await UserManager.AllUsersAsync();
            var roles = await RoleManager.AllRolesAsync();
            
            AdministrationViewModel administrationView = new AdministrationViewModel(users, roles);
            return View(administrationView);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetOtherRoles(string userId)
        {
            var roleList = await RoleManager.AllRolesAsync();
            var user = await UserManager.FindByIdAsync(userId);

            List<IdentityRole> roles = new List<IdentityRole>();
            if(roleList != null && roleList.Count > 0)
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddUserRole(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if(user.Roles == null)
                    {
                        user.Roles = new List<string>();
                    }
                    if(Request.Form.Count > 0)
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
        [Authorize(Roles = "Admin")]
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

    }
}