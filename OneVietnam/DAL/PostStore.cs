using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OneVietnam.Common;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class PostStore
    {
        private readonly IMongoCollection<Post> _posts;

        public PostStore(IMongoCollection<Post> posts)
        {
            _posts = posts;
        }

        public async Task CreatePostAsync(Post p)
        {
            await _posts.InsertOneAsync(p);
        }

        public async Task UpdateAsync(Post post)
        {
            await _posts.ReplaceOneAsync(p => p.Id == post.Id, post, null);
        }

        public async Task<List<Post>> FindByUserIdAsync(string userId)
        {
            return await _posts.Find(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Post> FindByIdAsync(string id)
        {
            return await _posts.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _posts.DeleteOneAsync(p => p.Id == id);
        }

        public async Task<List<Post>> FindAllPostAsync()
        {
            return await _posts.Find(p => true).ToListAsync();
        }

        public async Task<List<Post>> FindAllPostAsync(BaseFilter filter)
        {
            if (filter.IsNeedPaging)
            {
                return await _posts.Find(p => p.DeletedFlag==false).Skip(filter.Skip).Limit(filter.ItemsPerPage).ToListAsync();
            }
            else
            {
                return await _posts.Find(p => p.DeletedFlag == false).ToListAsync();
            }
        }
        public async Task<List<Post>> FullTextSearch(string query)
        {
            //var filter = Query.Text(query).ToBsonDocument();
            var filter = Builders<Post>.Filter.Text(query);            
            return await _posts.Find(filter).ToListAsync();
        }
        public async Task<List<Post>> FindTop5PostAsync()
        {
                return await _posts.Find(p => p.DeletedFlag == false).Limit(5).ToListAsync();
        }

    }
}