using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MongoDB.Bson;
using MongoDB.Driver;
using OneVietnam.Common;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public partial class PostManager : AbstractManager<Post>
    {        
        
        public static PostManager Create(IdentityFactoryOptions<PostManager> options,
            IOwinContext context)
        {
            var manager =
                new PostManager(new PostStore(context.Get<ApplicationIdentityContext>().Posts));
            return manager;
        }   
           
        //OK
        public async Task<List<Post>> FindByUserId(string userId)
        {
            var filter = Builders<Post>.Filter.Eq("UserId", userId);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return await Store.FindAllAsync(filter).ConfigureAwait(false);
        }
        //OK                                
        public async Task<List<BsonDocument>> FullTextSearch(string query, BaseFilter filter)
        {
            var sort = Builders<Post>.Sort.MetaTextScore("TextMatchScore").Ascending("PublishDate");
            return await Store.FullTextSearch(query, filter, sort).ConfigureAwait(false);
        }             
        public PostManager(PostStore store) : base(store)
        {
        }
    }
}