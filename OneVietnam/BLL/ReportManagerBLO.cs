using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using OneVietnam.DAL;
using OneVietnam.DTL;

namespace OneVietnam.BLL
{
    public class ReportManager : IDisposable
    {
        private readonly ReportStore _reportStore;
        public ReportManager(ReportStore reportStore)
        {
            _reportStore = reportStore;
        }

        public static ReportManager Create(IdentityFactoryOptions<ReportManager> options,
            IOwinContext context)
        {
            var manager =
                new ReportManager(new ReportStore(context.Get<ApplicationIdentityContext>().Reports));
            return manager;
        }

        public Task CreateAsync(Report pReport)
        {
            return _reportStore.CreatAsync(pReport);
        }

        public Task<List<Report>> GetReportsAsync()
        {
            return _reportStore.GetReportsAsync();
        }
               

        public void Dispose()
        {
        }
    }
}
