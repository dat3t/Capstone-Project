using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class NotificationModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public bool Seen { get; set; }
        public string CreatedDate { get; set; }
    }
}
