using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [Display(Name = "Duy Trì Đăng Nhập")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật Khẩu Xác Nhận Không Trùng Khớp")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class UserViewModel
    {
        public string UserName { get; set; }

        public string SecurityStamp { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(ApplicationUser appUser)
        {
            UserName = appUser.UserName;
            Email = appUser.Email;
            EmailConfirmed = appUser.EmailConfirmed;
            SecurityStamp = appUser.SecurityStamp;
            PhoneNumber = appUser.PhoneNumber;
        }
    }
    //DEMO
    public class CreatePostViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PostType { get; set; }
        // logical delete        
    }
    //DEMO
    public class ShowPostViewModel
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        public int PostType { get; set; }
        // logical delete
        public bool DeletedFlag { get; set; }
        // finished or not
        public bool Status { get; set; }

        public ShowPostViewModel(Post post)
        {
            Username = post.Username;
            Title = post.Title;
            Description = post.Description;
            PublishDate = post.PublishDate;
            PostType = post.PostType;
            DeletedFlag = post.DeletedFlag;
            Status = post.Status;
        }
    }
    //DEMO   
}
