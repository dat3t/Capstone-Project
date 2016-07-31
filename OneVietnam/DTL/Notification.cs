using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
    public class Notification:IComparable<Notification>
    {        
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public bool Seen { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public Notification(string url, string avatar, string description)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Url = url;
            Avatar = avatar;
            Description = description;
            Seen = false;
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public Notification(string url, string description)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Url = url;
            Description = description;
            Avatar = Constants.DefaultAvatarLink;
            Seen = false;
            CreatedDate = DateTimeOffset.UtcNow;            
        }

        public int CompareTo(Notification other)
        {
            if (this.CreatedDate < other.CreatedDate)
            {
                return -1;
            }
            else
            {
                if (this.CreatedDate == other.CreatedDate) return 0;
                return 1;
            }
        }
    }
}