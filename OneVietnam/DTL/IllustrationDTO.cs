using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
    public class Illustration
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        public string PhotoId { get; set; }

        public string Description { get; set; }
        public string PhotoLink { get; set; }
    }
}