using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using AspNet.Identity.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.Models;

namespace OneVietnam.DTL
{
    public class Report
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        [BsonIgnoreIfNull]
        public string PostId { get; set; }

        [BsonIgnoreIfNull]
        public string PostTitle { get; set; }

        public string UserId { get; set; }

        public string ReportDescription { get; set; }

        public bool ReportStatus { get; set; }
    }
}