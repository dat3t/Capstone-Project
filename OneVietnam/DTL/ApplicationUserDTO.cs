using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
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

        //DEMO
        public void AddLocation(Location location)
        {
            Location = location;
            
        }
    }
}