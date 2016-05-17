using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.MongoDB;

namespace OneVietnam
{
    public class EnsureAuthIndexes
    {
        public static void Exist()
        {
            var context = ApplicationIdentityContext.Create();
            IndexChecks.EnsureUniqueIndexOnUserName(context.Users);
            IndexChecks.EnsureUniqueIndexOnRoleName(context.Roles);
        }
    }
}