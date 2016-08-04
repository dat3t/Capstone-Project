using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class CommentorViewModel
    {
        public string Username { get; set; }
        public string Avatar { get; set; }
    }

    public class CommentViewModel
    {
        public string UserId { get; set; }
        public string Commentor { get; set; }
        public string Url { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
    }
}