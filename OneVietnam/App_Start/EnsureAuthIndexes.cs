using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam
{
    public class EnsureAuthIndexes
    {
        public static void Exist()
        {
            var context = ApplicationIdentityContext.Create();
            IndexChecks.EnsureUniqueIndexOnEmail(context.Users);
            IndexChecks.EnsureUniqueIndexOnRoleName(context.Roles);
            context.Users.Indexes.CreateOne(Builders<ApplicationUser>.IndexKeys.Ascending("UserName"));
            context.Posts.Indexes.CreateOne(Builders<Post>.IndexKeys.Ascending("UserId"));
            var options = new CreateIndexOptions()
            {
                Weights = new BsonDocument { { "Title", 2 }, { "Description", 1 }, { "Tags.TagText", 3 }, { "Illustrations.Description", 1 } }
            };
            context.Posts.Indexes.CreateOne(Builders<Post>.IndexKeys.Text("Title").Text("Description").Text("Illustrations.Description").Text("Tags.TagText"), options);
        }
    }
}