using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class SearchModel
    {
        List<PostViewModel> posts;
        List<UserViewModel> users;

        public SearchModel(List<PostViewModel> p, List<UserViewModel> u)
        {
            posts = p;
            users = u;
        }
    }
    
}