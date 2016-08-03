using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;
using OneVietnam.BLL;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class IconStore:AbstractStore<Icon>
    {                
        public IconStore(IMongoCollection<Icon> collection) : base(collection)
        {
        }

        public override async Task UpdateAsync(Icon instance)
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

        public override async Task UpdateAsync(Icon instance, bool upsert)
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