using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.Models;

namespace OneVietnam.DTL
{
    public class Post : BaseMongoDocument
    {        
        public string UserId { get; set; }        
        public string Title { get; set; }
        [BsonIgnoreIfNull]
        public string Description { get; set; }        
        [BsonIgnoreIfNull]
        public List<Illustration> Illustrations { get; set; }
        public int PostType { get; set; }        
        public bool Status { get; set; }
        public bool LockedFlag { get; set; }
        [BsonIgnoreIfNull]
        public Location PostLocation { get; set; }                
        [BsonIgnoreIfNull]
        public List<Report> Reports { get; set; }
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
            Illustrations = pView.Illustrations;
            Tags = pView.Tags;
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
            Illustrations = pView.Illustrations;
            Tags = pView.Tags;
            Reports = pView.Reports;
        }

        public void AddReport(Report pReport)
        {
            if (Reports == null)
            {
                Reports = new List<Report>();
            }
            Reports.Add(pReport);
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
}