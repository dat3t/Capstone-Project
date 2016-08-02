using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Bson;
using MongoDB.Driver;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public class ReportManager : AbstractManager<Report>
    {        

        public static ReportManager Create(IdentityFactoryOptions<ReportManager> options,
            IOwinContext context)
        {
            var manager =
                new ReportManager(new ReportStore(context.Get<ApplicationIdentityContext>().Reports));
            return manager;
        }                              
        public ReportManager(AbstractStore<Report> store) : base(store)
        {
        }

        public async Task<List<Report>> SearchRepostMultipleQuery(DateTimeOffset? createdDateFrom, DateTimeOffset? createdDateTo, string status)
        {
            var builder = Builders<Report>.Filter;
            FilterDefinition<Report> filter = null;            
            if (createdDateFrom != null)
            {
                var dateFrpm = builder.Gte("CreatedDate", createdDateFrom);
                filter = dateFrpm;
            }

            if (createdDateTo != null)
            {
                var dateTo = builder.Lt("CreatedDate", createdDateTo);
                if (filter == null)
                {
                    filter = dateTo;
                }
                else
                {
                    filter = filter & dateTo;
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                var statusFilter = builder.Eq("Status", status);
                if (filter == null)
                {
                    filter = statusFilter;
                }
                else
                {
                    filter = filter & statusFilter;
                }
            }
            return await Store.FindAllAsync(filter);
        }
    }
}
