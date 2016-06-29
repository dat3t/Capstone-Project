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
        public string PhotoId { get; set; }

        public string Description { get; set; }
        public string PhotoLink { get; set; }

        public Illustration()
        {
        }

        public Illustration(string pStrPhotoId, string pStrDescription, string pStrPhotoLink)
        {
            PhotoId = pStrPhotoId;
            Description = pStrDescription;
            PhotoLink = pStrPhotoLink;
        }
        
}
}