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
    public class Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
        [BsonIgnoreIfNull]
        public List<Illustration> Illustrations { get; set; }
        public int PostType { get; set; }
        // logical delete
        public bool DeletedFlag { get; set; }
        // finished or not
        public bool Status { get; set; }
        public Post(CreatePostViewModel pView)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Title = pView.Title;
            Description = pView.Description;
            PublishDate = DateTimeOffset.Now;
            PostType = pView.PostType;
            DeletedFlag = false;
            Status = false;
        }
    }
}