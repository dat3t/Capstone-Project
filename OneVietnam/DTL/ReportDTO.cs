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
        public string UserId { get; set; }
        [BsonIgnoreIfNull]
        public string PostId { get; set; }
        public string ReportDescription { get; set; }

        public bool ReportStatus { get; set; }

        public Report()
        {
        }

        public Report(string pUserId, string pPostId, string pDescription)
        {
            Id = ObjectId.GenerateNewId().ToString();
            UserId = pUserId;
            PostId = pPostId;
            ReportDescription = pDescription;
            ReportStatus = true;
        }

        public Report(Report pReport)
        {
            Id = pReport.Id;
            UserId = pReport.UserId;
            if(pReport.PostId != null)
            {
                PostId = pReport.PostId;
            }            
            ReportDescription = pReport.ReportDescription;
            ReportStatus = pReport.ReportStatus;
        }
    }
}