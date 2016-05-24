using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public int Gender { get; set; }
        [BsonIgnoreIfNull]
        public Location Location { get; set; }
        [BsonIgnoreIfNull]
        public List<Post> Posts { get; set; }

        public void AddPost(Post p)
        {
            if (Posts == null)
            {
                Posts = new List<Post>();
            }
            Posts.Add(p);
        }
    }
    public class Illustration
    {
        public string Description { get; set; }
        public string PhotoLink { get; set; }
    }
    public class Location
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }
        public string Address { get; set; }
    }
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