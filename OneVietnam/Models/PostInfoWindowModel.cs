using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class PostInfoWindowModel
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string postId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Location PostLocation { get; set; }
        public int PostType { get; set; }
        public List<Illustration> Illustrations { get; set; }
        public string TimeInterval { get; set; }
  
    }
}