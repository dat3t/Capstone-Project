using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class TagStore : AbstractStore<DTL.Tag>
    {        
        
        public override async Task UpdateAsync(DTL.Tag instance, bool upsert)
        {
            try
            {
                var option = new UpdateOptions() { IsUpsert = upsert };
                await Collection.ReplaceOneAsync(t => t.Id == instance.Id, instance, option).ConfigureAwait(false);
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

        //public Task<List<DTL.Tag>> FindTagByValueAsync(string pTagValue)
        //{
        //    return _tags.Find(u => u.TagValue.Contains(pTagValue)).ToListAsync();
        //}

        public override async Task UpdateAsync(DTL.Tag instance)
        {
            try
            {
                await Collection.ReplaceOneAsync(t => t.Id == instance.Id, instance, null);
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

        public TagStore(IMongoCollection<DTL.Tag> collection) : base(collection)
        {
        }
    }
}