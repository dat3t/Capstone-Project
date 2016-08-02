

using System;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.DTL;

namespace OneVietnam.Models
{
    public class ReportViewModal
    {       
        public string Id { get; set; }

        [BsonIgnoreIfNull]
        public string PostId { get; set; }
        [BsonIgnoreIfNull]
        public string PostTile { get; set; }
        
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string ReportDescription { get; set; }

        public string Status { get; set; }

        public bool DeletedFlag { get; set; }

        [BsonIgnoreIfNull]
        public string HandlerId { get; set; }

        [BsonIgnoreIfNull]
        public string HandlerName { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [BsonIgnoreIfNull]
        public DateTimeOffset? CloseDate { get; set; }
        public ReportViewModal()
        {

        }
        public ReportViewModal(string pPostId, string pUserId, string pDescription)
        {
            PostId = pPostId;
            UserId = pUserId;
            ReportDescription = pDescription;
        }

        public ReportViewModal(string pPostId, string pUserId)
        {
            PostId = pPostId;
            UserId = pUserId;            
        }

        public ReportViewModal(Report pReport)
        {
            Id = pReport.Id;
            UserId = pReport.UserId;
            ReportDescription = pReport.ReportDescription;
            if (!string.IsNullOrWhiteSpace(pReport.PostId))
            {
                PostId = pReport.PostId;
            }
            if (pReport.HandlerId != null)
            {
                HandlerId = pReport.HandlerId;
            }
            Status = pReport.Status;
            
            if (pReport.CloseDate != null)
            {
                CloseDate = pReport.CloseDate;
            }
            CreatedDate = pReport.CreatedDate;
            DeletedFlag = pReport.DeletedFlag;

        }

        public ReportViewModal(Report pReport, ApplicationUser pUser)
        {
            Id = pReport.Id;
            UserId = pReport.UserId;
            HandlerName = pUser.UserName;
            ReportDescription = pReport.ReportDescription;
            if (!string.IsNullOrWhiteSpace(pReport.PostId))
            {
                PostId = pReport.PostId;
            }
            if (pReport.HandlerId != null)
            {
                HandlerId = pReport.HandlerId;
            }
            Status = pReport.Status;

            if (pReport.CloseDate != null)
            {
                CloseDate = pReport.CloseDate;
            }
            CreatedDate = pReport.CreatedDate;
            DeletedFlag = pReport.DeletedFlag;

        }

        public ReportViewModal(Report pReport, string pPostTitle, string pUserName, string pHandlerName)
        {
            Id = pReport.Id;
            UserId = pReport.UserId;
            if (!string.IsNullOrWhiteSpace(pHandlerName))
            {
                HandlerName = pHandlerName;
            }
            if (!string.IsNullOrWhiteSpace(pPostTitle))
            {
                PostTile = pPostTitle;
            }
            UserName = pUserName;

            ReportDescription = pReport.ReportDescription;
            if (!string.IsNullOrWhiteSpace(pReport.PostId))
            {
                PostId = pReport.PostId;
            }
            if (pReport.HandlerId != null)
            {
                HandlerId = pReport.HandlerId;
            }
            Status = pReport.Status;

            if (pReport.CloseDate != null)
            {
                CloseDate = pReport.CloseDate;
            }
            CreatedDate = pReport.CreatedDate;
            DeletedFlag = pReport.DeletedFlag;

        }

    }
}