using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.MongoDB;
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
            //context.Users.Indexes.CreateOne(Builders<ApplicationUser>.IndexKeys.Text("UserName").Text("Posts.Title"));                                    
        }
    }
}