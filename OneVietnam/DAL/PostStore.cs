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
    public class PostStore : AbstractStore<Post>
    {
                                                              
        public override async Task UpdateAsync(Post instance)
        {
            try
            {
                await Collection.ReplaceOneAsync(p => p.Id == instance.Id, instance, null);
            }
            catch (MongoConnectionException ex)
            {
                throw new MongoConnectionException(ex.ConnectionId, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override async Task UpdateAsync(Post instance, bool upsert)
        {
            try
            {
                var option = new UpdateOptions() {IsUpsert = upsert};
                await Collection.ReplaceOneAsync(p => p.Id == instance.Id, instance, option).ConfigureAwait(false);   
            }
            catch (MongoConnectionException ex)
            {
                throw new MongoConnectionException(ex.ConnectionId, ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PostStore(IMongoCollection<Post> store) : base(store)
        {
        }
    }
}