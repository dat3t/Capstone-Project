using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OneVietnam.Models;
using OneVietnam.Properties;
using AspNet.Identity.MongoDB;

namespace OneVietnam
{
    /// <summary>
    /// the context class, we can crate function which access to mongodb here
    /// </summary>
    public class ApplicationIdentityContext : IDisposable
    {
        public IMongoCollection<ApplicationUser> Users { get; set; }
        public IMongoCollection<IdentityRole> Roles { get; set; }

        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles)
        {
            Users = users;
            Roles = roles;
        }
        public static ApplicationIdentityContext Create()
        {
            // todo add settings where appropriate to switch server & database in your own application            
            var client = new MongoClient(Settings.Default.OneVietnamConnectionString);
            var database = client.GetDatabase(Settings.Default.OneVietnamDatabaseName);
            var users = database.GetCollection<ApplicationUser>("users");
            var roles = database.GetCollection<IdentityRole>("roles");
            return new ApplicationIdentityContext(users, roles);
        }

        public Task<List<IdentityRole>> AllRolesAsync()
        {
            return Roles.Find(r => true).ToListAsync();
        }
        public void Dispose()
        {            
        }
    }
}