using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using OneVietnam.DTL;

namespace OneVietnam.DAL
{
    public class RoleStore : RoleStore<IdentityRole>
    {
        private readonly IMongoCollection<IdentityRole> _roles;
        public RoleStore(IMongoCollection<IdentityRole> roles) : base(roles)
        {
            _roles = roles;
        }
        public Task<List<IdentityRole>> AllRolesAsync()
        {
            return _roles.Find(r => true).ToListAsync();
        }
    }
}