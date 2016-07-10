using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using OneVietnam.DTL;
using OneVietnam.Models;

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

        public List<AddLocationViewModel> GetInfoForInitMap(List<ApplicationUser> userList)
        {
            List<AddLocationViewModel> infoForInitMap = new List<AddLocationViewModel>();
            AddLocationViewModel viewModel;
            foreach (ApplicationUser user in userList)
            {

                viewModel = new AddLocationViewModel(user.Location, user.Id, user.Gender, user.Posts);
                infoForInitMap.Add(viewModel);
            }

            return infoForInitMap;
        }

        public List<Location> GetLocationListAsync(List<ApplicationUser> userList)
        {
            return userList.Select(user => user.Location).ToList();
        }

        public List<List<Post>> GetPostListAsync(List<ApplicationUser> userList)
        {
            return userList.Select(user => user.Posts).ToList();
        }

        public async Task<List<ApplicationUser>> TextSearchByUserName(string query)
        {
            var filter = new BsonDocument {{"UserName", new BsonDocument {{"$regex", query}, {"$options", "i"}}}};

            var result = await _users.Find(filter).ToListAsync();
            return result;
        }
    }
}