using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AspNet.Identity.MongoDB;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class AdministrationViewModel
    {
        public List<UserManagementViewModel> Users { get; set; }

        public List<RoleViewModel> Roles { get; set; }

        public List<AdminPostViewModel> Posts { get; set; }

        public List<ReportViewModel> Reports { get; set; }

        public AdministrationViewModel() { }

        public AdministrationViewModel(List<ApplicationUser> pUsers, List<IdentityRole> pRoles)
        {
            if (pUsers != null && pUsers.Count > 0)
            {
                Users = new List<UserManagementViewModel>();
                foreach (var user in pUsers)
                {
                    UserManagementViewModel userView = new UserManagementViewModel(user);
                    Users.Add(userView);
                }
            }

            if (pRoles != null && pRoles.Count > 0)
            {                
                Roles = new List<RoleViewModel>();
                foreach (var role in pRoles)
                {
                    RoleViewModel roleView = new RoleViewModel(role);
                    Roles.Add(roleView);
                }
            }

        }

        public AdministrationViewModel(List<ApplicationUser> pUsers, List<IdentityRole> pRoles, List<Post> pPosts, List<Report> pReports)
        {
            if (pUsers != null && pUsers.Count > 0)
            {
                Users = new List<UserManagementViewModel>();
                foreach (var user in pUsers)
                {
                    UserManagementViewModel userView = new UserManagementViewModel(user);
                    Users.Add(userView);
                }
            }

            if (pPosts != null && pPosts.Count > 0)
            {
                Posts = new List<AdminPostViewModel>();
                foreach (var post in pPosts)
                {
                    AdminPostViewModel postView = new AdminPostViewModel(post);
                    Posts.Add(postView);
                }
            }

            if (pRoles != null && pRoles.Count > 0)
            {
                Roles = new List<RoleViewModel>();
                foreach (var role in pRoles)
                {
                    RoleViewModel roleView = new RoleViewModel(role);
                    Roles.Add(roleView);
                }
            }

            if(pReports != null && pReports.Count > 0)
            {
                Reports = new List<ReportViewModel>();
                foreach (var report in pReports)
                {
                    ReportViewModel reportView = new ReportViewModel(report);
                    Reports.Add(reportView);
                }
            }

        }

        public AdministrationViewModel(List<ApplicationUser> pUsers, List<IdentityRole> pRoles, List<Post> pPosts, List<ReportViewModel> pReports)
        {
            if (pUsers != null && pUsers.Count > 0)
            {
                Users = new List<UserManagementViewModel>();
                foreach (var user in pUsers)
                {
                    UserManagementViewModel userView = new UserManagementViewModel(user);
                    Users.Add(userView);
                }
            }

            //if (pPosts != null && pPosts.Count > 0)
            //{
            //    Posts = new List<AdminPostViewModel>();
            //    foreach (var post in pPosts)
            //    {
            //        AdminPostViewModel postView = new AdminPostViewModel(post);
            //        Posts.Add(postView);
            //    }
            //}

            if (pRoles != null && pRoles.Count > 0)
            {
                Roles = new List<RoleViewModel>();
                foreach (var role in pRoles)
                {
                    RoleViewModel roleView = new RoleViewModel(role);
                    Roles.Add(roleView);
                }
            }

            if (pReports != null && pReports.Count > 0)
            {
                Reports = pReports;
            }

        }
    }

    public class UserManagementViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public List<string> Roles { get; set; }       

        public bool LockedFlag { get; set; }

        public bool DeletedFlag { get; set; }

        public UserManagementViewModel() { }

        public UserManagementViewModel(ApplicationUser pUser)
        {
            Id = pUser.Id;
            UserName = pUser.UserName;
            CreatedDate = pUser.CreatedDate;
            if (pUser.Roles != null)
            {
                Roles = pUser.Roles;
            }            
            LockedFlag = pUser.LockedFlag;
            DeletedFlag = pUser.DeletedFlag;
        }

    }


    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }

        public RoleViewModel()
        {
        }

        public RoleViewModel(IdentityRole pRole)
        {
            Id = pRole.Id;
            Name = pRole.Name;
        }
    }

    public class UserRoleViewModel
    {
        public string Id { get; set; }
        public string Role { get; set; }

        public UserRoleViewModel() { }

        public UserRoleViewModel(string pUserId, string pRole)
        {
            Id = pUserId;
            Role = pRole;
        }
    }
    
}