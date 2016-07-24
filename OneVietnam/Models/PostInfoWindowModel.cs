using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class PostInfoWindowModel
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string postId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Address { get; set; }


    }
}