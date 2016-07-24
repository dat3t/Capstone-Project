using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class MessageStore:AbstractStore<Message>
    {
        public MessageStore(IMongoCollection<Message> collection) : base(collection)
        {

        }

        public override async Task UpdateAsync(Message instance)
        {
            try
            {
                await Collection.ReplaceOneAsync(m => m.Id == instance.Id, instance, null).ConfigureAwait(false);
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

        public override async Task UpdateAsync(Message instance, bool upsert)
        {
            try
            {
                var option = new UpdateOptions() { IsUpsert = upsert };
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
    }
}