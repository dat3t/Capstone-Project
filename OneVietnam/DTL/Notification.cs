using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.DTL
{
    public class Notification
    {
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public bool Seen { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public Notification(string url, string avatar, string description)
        {
            Url = url;
            Avatar = avatar;
            Description = description;
            Seen = false;
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public Notification(string url, string description)
        {
            Url = url;
            Description = description;
            Avatar = Constants.DefaultAvatarLink;
            Seen = false;
            CreatedDate = DateTimeOffset.UtcNow;            
        }
    }
}