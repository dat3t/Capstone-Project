﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.DAL
{
    public class UserStore : UserStore<ApplicationUser>
    {
        private readonly IMongoCollection<ApplicationUser> _users;
        public UserStore(IMongoCollection<ApplicationUser> users) : base(users)
        {
            _users = users;
        }

        public Task<List<ApplicationUser>> AllUsersAsync()
        {
            return _users.Find(u => true).ToListAsync();
        }

        public async Task<List<ApplicationUser>> FindUsersByRoleAsync(IdentityRole role)
        {
            return await _users.Find(u => u.Roles.Contains(role.Name)).ToListAsync();
        }                

        public async Task<List<ApplicationUser>> TextSearchUsers(string query)
        {
            //var filter = new BsonDocument {{"UserName", new BsonDocument {{"$regex", query}, {"$options", "i"}}}, { "Email", new BsonDocument { { "$regex", query }, { "$options", "i" } } } };            
            var builder = Builders<ApplicationUser>.Filter;
            var filter = builder.Regex("UserName", new BsonRegularExpression(query, "i")) | builder.Regex("Email", new BsonRegularExpression(query, "i"));            
            return await _users.Find(filter).ToListAsync();            
        }

    }
}