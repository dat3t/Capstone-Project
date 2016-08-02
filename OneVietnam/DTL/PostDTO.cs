
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.Models;

namespace OneVietnam.DTL
{
    public class Post : BaseMongoDocument
    {        
        public string UserId { get; set; }        
        public string Title { get; set; }        
        public string Description { get; set; }        
        [BsonIgnoreIfNull]
        public List<Illustration> Illustrations { get; set; }
        public int PostType { get; set; }        
        public bool Status { get; set; }
        public bool LockedFlag { get; set; }        
        public Location PostLocation { get; set; }                
        [BsonIgnoreIfNull]
        public List<Tag> Tags { get; set; }
        [BsonIgnoreIfNull]
        public double? TextMatchScore { get; set; }

        public Post()
        {
            
        }
        public Post(CreatePostViewModel pView)
        {            
            Id = ObjectId.GenerateNewId().ToString();            
            Title = pView.Title;
            UserId = pView.UserId;
            Description = pView.Description;            
            PostType = pView.PostType;
            DeletedFlag = false;
            Status = true;
            LockedFlag = false;
            PostLocation = pView.PostLocation;
            if (pView.Illustrations != null && pView.Illustrations.Count > 0)
            {
                Illustrations = pView.Illustrations;
            }
            if (pView.Tags != null && pView.Tags.Count > 0)
            {
                Tags = pView.Tags;
            }
        }

        public Post(PostViewModel pView)
        {
            Id = pView.Id;            
            Title = pView.Title;
            UserId = pView.UserId;
            Description = pView.Description;
            CreatedDate = pView.CreatedDate;
            PostType = pView.PostType;
            DeletedFlag = pView.DeletedFlag;
            Status = pView.Status;
            PostLocation = pView.PostLocation;
            if (pView.Illustrations != null && pView.Illustrations.Count > 0)
            {
                Illustrations = pView.Illustrations;
            }
            if (pView.Tags != null && pView.Tags.Count > 0)
            {
                Tags = pView.Tags;
            }
                       
        }


        public Post(CreateAdminPostViewModel pView)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Title = pView.Title;            
            Description = pView.Description;
            CreatedDate = DateTimeOffset.UtcNow;
            PostType = 0;
            DeletedFlag = false;
            Status = true;
            LockedFlag = false;
            if (pView.Illustrations != null && pView.Illustrations.Count > 0)
            {
                Illustrations = pView.Illustrations;
            }            
        }

        public Post(AdminPostViewModel pView)
        {
            Id = pView.Id;
            Title = pView.Title;
            UserId = pView.UserId;
            Description = pView.Description;
            CreatedDate = pView.CreatedDate;
            PostType = pView.PostType;
            DeletedFlag = pView.DeletedFlag;
            Status = pView.Status;
            if (pView.Illustrations != null && pView.Illustrations.Count > 0)
            {
                Illustrations = pView.Illustrations;
            }
        }
    }
}