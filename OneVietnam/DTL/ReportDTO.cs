using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        [BsonIgnoreIfNull]
        public string HandlerId { get; set; }

        public bool ReportStatus { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        [BsonIgnoreIfNull]
        public DateTimeOffset? CloseDate { get; set; }

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
            CreateDate = DateTime.Now;            
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
            if(pReport.HandlerId != null)
            {
                HandlerId = pReport.HandlerId;
            }            
            CreateDate = pReport.CreateDate;
            if(pReport.CloseDate != null)
            {
                CloseDate = pReport.CloseDate;
            }
        }
    }
}