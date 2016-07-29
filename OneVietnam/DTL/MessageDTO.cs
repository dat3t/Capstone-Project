using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneVietnam.DTL
{
    public class Message:IComparable<Message>
    {        
        public string Content { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int Type { get; set; }
        
        public Message(string content, int type)
        {            
            Content = content;
            Type = type;
            CreatedDate = DateTimeOffset.UtcNow;
        }

        public int CompareTo(Message other)
        {
            if (this.CreatedDate < other.CreatedDate)
            {
                return -1;
            }
            else
            {
                if (this.CreatedDate == other.CreatedDate) return 0;
                return 1;
            }
        }
    }

    public class Conversation: IComparable<Conversation>
    {
        public List<Message> MessageList { get; set; }
        public int CompareTo(Conversation other)
        {
            if (this.MessageList[this.MessageList.Count - 1].CreatedDate <
                other.MessageList[other.MessageList.Count - 1].CreatedDate)
            {
                return -1;
            }
            else
            {
                return this.MessageList[this.MessageList.Count - 1].CreatedDate ==
                       other.MessageList[other.MessageList.Count - 1].CreatedDate ? 0 : 1;
            }
        }

        public DateTimeOffset UpdatedDate => MessageList[MessageList.Count - 1].CreatedDate;

        public string LastestMessage => MessageList[MessageList.Count - 1].Content;

        public int LastestType => MessageList[MessageList.Count - 1].Type;
        public bool Seen { get; set; } = false;
    }
}