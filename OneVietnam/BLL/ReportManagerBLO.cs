using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
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
    }
}
