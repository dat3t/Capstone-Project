using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.Common;

namespace OneVietnam.DTL
{
    public class Report :BaseMongoDocument
    {                      
        public string UserId { get; set; }
        [BsonIgnoreIfNull]
        public string PostId { get; set; }
        public string ReportDescription { get; set; }
        [BsonIgnoreIfNull]
        public string HandlerId { get; set; }

        public string Status { get; set; }        

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
            Status = ReportStatus.Open.ToString();
            DeletedFlag = false;
            CreatedDate = DateTimeOffset.UtcNow;            
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
            Status = pReport.Status;
            if(pReport.HandlerId != null)
            {
                HandlerId = pReport.HandlerId;
            }            
            CreatedDate = pReport.CreatedDate;
            if(pReport.CloseDate != null)
            {
                CloseDate = pReport.CloseDate;
            }
            DeletedFlag = pReport.DeletedFlag;
        }
    }
}