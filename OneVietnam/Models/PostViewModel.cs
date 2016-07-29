using System;
using System.Collections.Generic;
using OneVietnam.DTL;
using System.ComponentModel.DataAnnotations;
using OneVietnam.Common;

namespace OneVietnam.Models
{
    public class CreatePostViewModel
    {

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]        
        [Display(Name = "Người đăng")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "{0} chưa được chọn.")]
        [Range((int)PostTypeEnum.Accommodation, (int)PostTypeEnum.Sos, ErrorMessage = "{0} chưa được chọn.")]
        [Display(Name = "Loại bài đăng")]
        public int PostType { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [Display(Name = "Địa chỉ bài đăng")]
        public Location PostLocation { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public List<Illustration> Illustrations { get; set; }

        public List<Tag> Tags { get; set; }    

    }
    public class PostViewModel
    {
        public string Id { get; set; }     

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Người đăng")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Người đăng")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.Text)]
        [Display(Name = "Mô tả")]
        public string AvartarLink { get; set; }
       
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [DataType(DataType.DateTime, ErrorMessage = "{0} không đúng định dạng thời gian")]
        [Display(Name = "Ngày tạo")]
        public DateTimeOffset CreatedDate { get; set; }

        [Required(ErrorMessage = "{0} chưa được chọn.")]
        [Range((int)PostTypeEnum.Accommodation, (int)PostTypeEnum.Sos, ErrorMessage = "{0} chưa được chọn")]
        [Display(Name = "Loại bài đăng")]
        public int PostType { get; set; }

        [Required(ErrorMessage = "{0} chưa được chỉ định.")]        
        [Display(Name = "Cờ xác nhận xóa bài")]
        public bool DeletedFlag { get; set; }

        [Required(ErrorMessage = "{0} chưa được chỉ định.")]
        [Display(Name = "Tình trạng bài đăng")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "{0} chưa được chỉ định.")]
        [Display(Name = "Cờ xác nhận khóa bài")]
        public bool LockedFlag { get; set; }

        [Required(ErrorMessage = "{0} chưa được điền.")]
        [Display(Name = "Địa chỉ bài đăng")]
        public Location PostLocation { get; set; }
        public List<Illustration> Illustrations { get; set; }
        public List<Tag> Tags { get; set; }

        public PostViewModel()
        {
        }
        public PostViewModel(Post post)
        {
            Id = post.Id;
            UserId = post.UserId;     
            Title = post.Title;
            Description = post.Description;
            CreatedDate = post.CreatedDate;
            PostType = post.PostType;
            DeletedFlag = post.DeletedFlag;
            Status = post.Status;
            LockedFlag = post.LockedFlag;           
            PostLocation = post.PostLocation;
            Tags = post.Tags;
            Illustrations = post.Illustrations;
        }
        public PostViewModel(Post post, string pUserName,string postUserAvatar)
        {
            AvartarLink = postUserAvatar;
            Id = post.Id;
            UserId = post.UserId;
            UserName = pUserName;
            Title = post.Title;
            Description = post.Description;
            CreatedDate = post.CreatedDate;
            PostType = post.PostType;
            DeletedFlag = post.DeletedFlag;
            Status = post.Status;
            LockedFlag = post.LockedFlag;
            PostLocation = post.PostLocation;
            Tags = post.Tags;
            Illustrations = post.Illustrations;
        }
    }
}