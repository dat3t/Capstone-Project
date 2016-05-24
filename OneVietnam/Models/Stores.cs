using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;

namespace OneVietnam.Models
{
    public class UserStore : UserStore<ApplicationUser>
    {
        private readonly IMongoCollection<ApplicationUser> _users;
        public UserStore(IMongoCollection<ApplicationUser> users) : base(users)
        {
            _users = users;
        }

        public Task<List<ApplicationUser>> AllUsersAsync()
        {
            return _users.Find(u => true).ToListAsync();
        }

        public Task AddPostAsync(ApplicationUser user, Post post)
        {
            user.AddPost(post);
            return Task.FromResult(0);
        }

        public List<Post> GetPostsAsync(ApplicationUser user)
        {
            return  user.Posts;
        }        
    }
}