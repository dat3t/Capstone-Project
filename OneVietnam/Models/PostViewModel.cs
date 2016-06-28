using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class CreatePostViewModel
    {
        public string Title { get; set; }

        public string UserName { get; set; }

        public int PostType { get; set; }

        public Location UserLocation { get; set; }

        public Location PostLocation { get; set; }

        public string Description { get; set; }

        public List<Illustration> Illustrations { get; set; }

        public List<Tag> Tags { get; set; }

    }
    public class ShowPostViewModel
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        public int PostType { get; set; }
        public bool DeletedFlag { get; set; }
        public bool Status { get; set; }        
        public Location UserLocation { get; set; }
        public Location PostLocation { get; set; }
        public List<Illustration> Illustrations { get; set; }
        public List<Tag> Tags { get; set; }

        public List<Report> Reports { get; set; }
        public ShowPostViewModel()
        {
        }
        public ShowPostViewModel(Post post)
        {
            UserName = post.Username;
            Title = post.Title;
            Description = post.Description;
            PublishDate = post.PublishDate;
            PostType = post.PostType;
            DeletedFlag = post.DeletedFlag;
            Status = post.Status;
            UserLocation = post.UserLocation;
            PostLocation = post.PostLocation;
            Tags = post.Tags;
            Reports = post.Reports;            
        }
    }
}