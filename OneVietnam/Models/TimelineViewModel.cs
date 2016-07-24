using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OneVietnam.BLL;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class TimelineViewModel
    {
        public string Id { get; set; }
        public string AvatarLink { get; set; }
        public string UserName { get; set; }
        public List<PostViewModel> PostList { get; set; }
        public TwoFacterViewModel Setting { get; set; }

        public TimelineViewModel() { }

        public TimelineViewModel(ApplicationUser user, List<Post> posts)
        {
            Id = user.Id;
            UserName = user.UserName;
            //AvatarLink = user.AvatarLink; TODO           
            if (posts != null)
            {
                PostList = new List<PostViewModel>();
                foreach (var post in posts)
                {
                    PostViewModel postView = new PostViewModel(post, user.UserName);
                    PostList.Add(postView);
                }
            }
            Setting = new TwoFacterViewModel(user);
        }
    }


    //ThamDTH Create
    public class UserProfileViewModel
    {
        public string Id { get; set; }        
        public int Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Location Address { get; set; }
        public UserProfileViewModel()
        {
        }

        public UserProfileViewModel(ApplicationUser user)
        {
            Id = user.Id;                
            Gender = user.Gender;            
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Address = user.Location;
        }

    }

    //ThamDTH Create
    public class TwoFacterViewModel
    {
        public string Id { get; set; }
        public bool TwoFacterEnabled { get; set; }

        public TwoFacterViewModel()
        {
        }

        public TwoFacterViewModel(ApplicationUser user)
        {
            Id = user.Id;
            TwoFacterEnabled = user.TwoFactorEnabled;
        }
    }

    public class ChangePasswordViewModel
    {        

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận lại mật khẩu không khớp với nhau.")]
        public string ConfirmPassword { get; set; }

        public ChangePasswordViewModel()
        {
            
        }
        public ChangePasswordViewModel(string pOldPass, string pNewPass, string pConfirmPass)
        {
            OldPassword = pOldPass;
            NewPassword = pNewPass;
            ConfirmPassword = pConfirmPass;
        }
    }

}