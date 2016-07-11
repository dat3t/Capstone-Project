using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
    public class Icon
    {        
        [BsonId]
        public int IconId { get; set; }
        public string IconText { get; set; }
        public string IconImage { get; set; }
        public string IconType { get; set; }

        public Icon()
        {
        }

        public Icon(int pIconId, string pIconText, string pIconImage, string pIconType)
        {
            IconId = pIconId;
            IconText = pIconText;
            IconImage = pIconImage;
            IconType = pIconType;
        }

    }
    
}