using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class ReportStore
    {
        private readonly IMongoCollection<Report> _reports;

        public ReportStore(IMongoCollection<Report> pReports)
        {
            _reports = pReports;
        }

        public Task CreatAsync(Report pReport)
        {
            return _reports.InsertOneAsync(pReport);
        }

        public virtual Task UpdateAsync(Report pReport)
        {
            return _reports.ReplaceOneAsync(c => c.Id == pReport.Id, pReport, null);
        }

        public Task<List<Report>> GetReportsAsync()
        {
            return _reports.Find(u => true).ToListAsync();
        }


    }
}