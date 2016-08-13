using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class ReportStore : AbstractStore<Report>
    {                
        public override Task UpdateAsync(Report instance)
        {
            return Collection.ReplaceOneAsync(c => c.Id == instance.Id, instance, null);
        }

        public override async Task UpdateAsync(Report instance, bool upsert)
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
        public ReportStore(IMongoCollection<Report> collection) : base(collection)
        {
        }
    }
}