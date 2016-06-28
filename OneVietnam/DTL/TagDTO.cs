using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
    public class Tag
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        
        public string TagValue { get; set; }
                
    }
}