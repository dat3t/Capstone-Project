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
    //[Authorize(Roles = "Admin")]
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



        public async Task<ActionResult> ShowAdminPanel()
        {
            var users = await UserManager.AllUsersAsync();
            var roles = await RoleManager.AllRolesAsync();
            AdministrationViewModel administrationView = new AdministrationViewModel(users, roles);
            return View(administrationView);
        }

        [HttpPost]
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
        public async Task<ActionResult> RemoveRole(string roleId)
        {

            var role = await RoleManager.FindByIdAsync(roleId);
            var roleresult = await RoleManager.DeleteAsync(role);
            if (!roleresult.Succeeded)
            {
                ModelState.AddModelError("", roleresult.Errors.First());                
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

    }
}