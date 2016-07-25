
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class TimelineViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Ảnh đại diện")]
        public string AvatarLink { get; set; }
        public string UserName { get; set; }
        public List<PostViewModel> PostList { get; set; }
        public TwoFacterViewModel Setting { get; set; }

        public UserProfileViewModel Profile { get; set; }

        public TimelineViewModel() { }

        public TimelineViewModel(ApplicationUser user, List<Post> posts)
        {
            Id = user.Id;
            if (user.UserName != null)
            {
                UserName = user.UserName;
            }            
            if (user.Avatar != null)
            {
                AvatarLink = user.Avatar;
            }                    
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
            Profile = new UserProfileViewModel(user);
        }
    }


    //ThamDTH Create
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd/mm/yy", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày sinh")]
        public DateTimeOffset? DateOfBirth { get; set; }


        [Required(ErrorMessage = "{0} chưa được chọn.")]        
        [Display(Name = "Giới tính")]
        public int Gender { get; set; }
        
        
        [Required(ErrorMessage = "{0} không được để trống.")]        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} không được để trống.")]        
        [Display(Name = "Địa chỉ")]
        public string Location { get; set; }
        public UserProfileViewModel()
        {
        }

        public UserProfileViewModel(ApplicationUser user)
        {
            Id = user.Id;
            if (user.UserName != null)
            {
                UserName = user.UserName;
            }              
            Gender = user.Gender;            
            Email = user.Email;
            Location = user.Location.Address;
            if (user.PhoneNumber != null)
            {
                PhoneNumber = user.PhoneNumber;
            }
            if (user.DateOfBirth != null)
            {
                DateOfBirth = user.DateOfBirth;
            }
        }

    }

    //ThamDTH Create
    public class TwoFacterViewModel
    {
        public string Id { get; set; }
        public bool TwoFacterEnabled { get; set; }        
        public bool HasPassWord { get; set; }
        public TwoFacterViewModel()
        {
        }

        public TwoFacterViewModel(ApplicationUser user)
        {
            Id = user.Id;
            TwoFacterEnabled = user.TwoFactorEnabled;
            HasPassWord = user.PasswordHash != null;
        }
    }

    public class ChangePasswordViewModel
    {        

        [Required(ErrorMessage = "{0} không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} phải chứa ít nhất {2} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận lại không khớp với nhau.")]
        public string ConfirmPassword { get; set; }

        public ChangePasswordViewModel(){}        
    }

}