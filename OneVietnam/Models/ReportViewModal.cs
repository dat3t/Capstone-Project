

namespace OneVietnam.Models
{
    public class ReportViewModal
    {       
        public string PostId { get; set; }        

        public string UserId { get; set; }

        public string ReportDescription { get; set; }

        public bool ReportStatus { get; set; }

        public ReportViewModal()
        {

        }
        public ReportViewModal(string pPostId, string pUserId)
        {
            PostId = pPostId;
            UserId = pUserId;            
        }
        public ReportViewModal(string pPostId, string pUserId, string pDescription)
        {
            PostId = pPostId;
            UserId = pUserId;
            ReportDescription = pDescription;
        }

        
    }
}