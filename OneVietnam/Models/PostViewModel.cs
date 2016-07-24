using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneVietnam.DTL;
using System.ComponentModel.DataAnnotations;
namespace OneVietnam.Models
{
    public class CreatePostViewModel
    {
        
        public string Title { get; set; }
        
        public string UserId { get; set; }

        public int PostType { get; set; }        

        public Location PostLocation { get; set; }
                
        public string Description { get; set; }

        public List<Illustration> Illustrations { get; set; }

        public List<Tag> Tags { get; set; }

        public CreatePostViewModel()
        {
        }

        public CreatePostViewModel(List<Illustration> pIllustrations, List<Tag> pTags)
        {
            Illustrations = pIllustrations;
            Tags = pTags;
        }        

        public void AddIllustration(Illustration pIllustration)
        {
            if (Illustrations == null)
            {
                Illustrations = new List<Illustration>();
            }
            Illustrations.Add(pIllustration);
        }

        public void AddTags(Tag pTag)
        {
            if (Tags == null)
            {
                Tags = new List<Tag>();
            }
            Tags.Add(pTag);
        }

    }
    public class PostViewModel
    {
        public string Id { get; set; }     
        public string UserId { get; set; }
        public string Title { get; set; }
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
    }
}