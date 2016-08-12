using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.Models
{
    public class ConversationViewModel
    {        
        public string Id { get; set; }
        public string FriendName { get; set; }
        public string Avatar { get; set; }
        public string LastestMessage { get; set; }
        public string UpdatedDate { get; set; }
        public int LastestType { get; set; }
        public bool Seen { get; set; }
    }    
}