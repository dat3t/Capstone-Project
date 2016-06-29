using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
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

        public async Task<List<ApplicationUser>> FindUsersByRoleAsync(IdentityRole role)
        {
            return await _users.Find(u => u.Roles.Contains(role.Name)).ToListAsync();
        }

        public Task AddPostAsync(ApplicationUser user, Post post)
        {
            user.AddPost(post);
            return Task.FromResult(0);
        }

        public List<Post> GetPostsAsync(ApplicationUser user)
        {
            return user.Posts;
        }

        //DEMO
        public Task AddLocationAsync(ApplicationUser user, Location location)
        {
            user.AddLocation(location);
            return Task.FromResult(0);
        }

        public List<Location> GetLocationAsync(List<ApplicationUser> userList)
        {
            //user.AddLocation(location);
            List<Location> Locations = new List<Location>();
            foreach (ApplicationUser user in userList)
            {
                Locations.Add(user.Location);
            }
            return Locations;
        }
    }
}