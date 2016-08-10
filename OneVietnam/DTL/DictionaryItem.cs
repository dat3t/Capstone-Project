using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace OneVietnam.DTL
{
    public class DictionaryItem
    {
        public ObjectId Key { get; set; }
        public Notification Value { get; set; }
    }
}