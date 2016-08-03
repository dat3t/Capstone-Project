using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OneVietnam.Models;

namespace OneVietnam.DTL
{
    public class Icon:BaseMongoDocument
    {
        public int IconValue { get; set; }
        public string IconText { get; set; }
        public string IconImage { get; set; }
        public string IconType { get; set; }        
        public Icon(CreateIconModel model)
        {
            Id=ObjectId.GenerateNewId().ToString();
            IconValue = model.IconValue;
            IconText = model.IconText;
            IconImage = model.IconImage;
            IconType = model.IconType;
            CreatedDate = DateTimeOffset.UtcNow;
            DeletedFlag = false;
        }
    }
    
}