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
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string AvartarLink { get; set; }
       
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int PostType { get; set; }
        public bool DeletedFlag { get; set; }
        public bool Status { get; set; }

        public bool LockedFlag { get; set; }

        public Location PostLocation { get; set; }
        public List<Illustration> Illustrations { get; set; }
        public List<Tag> Tags { get; set; }

        public List<Report> Reports { get; set; }        

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
            Reports = post.Reports;
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
            Reports = post.Reports;
            Illustrations = post.Illustrations;
        }
    }
}