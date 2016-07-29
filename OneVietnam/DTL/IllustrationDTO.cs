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
        public string PhotoLink { get; set; }
        [BsonIgnoreIfNull]
        public string Description { get; set; }
        

        public Illustration()
        {
        }
        public Illustration(string pStrPhotoLink)
        {            
            PhotoLink = pStrPhotoLink;
        }
        public Illustration(string pStrPhotoLink, string pStrDescription)
        {
            Description = pStrDescription;
            PhotoLink = pStrPhotoLink;
        }

    }
}