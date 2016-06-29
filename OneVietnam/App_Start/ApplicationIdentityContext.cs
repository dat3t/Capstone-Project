﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OneVietnam.Models;
using OneVietnam.Properties;
using AspNet.Identity.MongoDB;
using Microsoft.Ajax.Utilities;
using OneVietnam.DTL;

namespace OneVietnam
{
    /// <summary>
    /// the context class, we can crate function which access to mongodb here
    /// </summary>
    public class ApplicationIdentityContext : IDisposable
    {
        public IMongoCollection<ApplicationUser> Users { get; set; }
        public IMongoCollection<IdentityRole> Roles { get; set; }
        public IMongoCollection<Country> Countries { get; set; }

        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles, IMongoCollection<Country> countries)
        {
            Users = users;
            Roles = roles;
            Countries = countries;
        }
        public static ApplicationIdentityContext Create()
        {
            // todo add settings where appropriate to switch server & database in your own application            
            var client = new MongoClient(Settings.Default.OneVietnamConnectionString);
            var database = client.GetDatabase(Settings.Default.OneVietnamDatabaseName);
            var users = database.GetCollection<ApplicationUser>("users");
            var roles = database.GetCollection<IdentityRole>("roles");
            var countries = database.GetCollection<Country>("countries");
            return new ApplicationIdentityContext(users, roles, countries);
        }
        public void Dispose()
        {
        }
    }
}