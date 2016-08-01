using System;
using System.Collections.Generic;
using System.Linq;
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
        public Location Location { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [BsonIgnoreIfNull]
        public string Avatar { get; set; }
        [BsonIgnoreIfNull]
        public string Cover { get; set; }
        [BsonIgnoreIfNull]
        public SortedList<string, Conversation> Conversations { get; set; }
        public SortedList<string,Notification> Notifications { get; set; }
        [BsonIgnoreIfNull]
        public List<Connection> Connections { get; set; }
        [BsonIgnoreIfNull]
        public DateTimeOffset? DateOfBirth { get; set; }
        public bool LockedFlag { get; set; }

        public bool DeletedFlag { get; set; }

        public int CountUnReadConversations()
        {
            return Conversations.Where((t, i) => !Conversations.ElementAt(i).Value.Seen).Count();
        }

        public int CountUnReadNotifications()
        {
            return this.Notifications.Where((t, i) => Notifications.Values[i].Seen == false).Count();
        }
    }
}