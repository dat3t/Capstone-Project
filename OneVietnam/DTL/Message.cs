using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.DTL
{
    public class Message
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Time { get; set; }
        public int Type { get; set; }

        public Message(string userId, string content, int type)
        {
            UserId = userId;
            Content = content;
            Type = type;
            Time = DateTimeOffset.UtcNow;
        }
    }
}