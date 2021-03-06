﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
    public class Tag : BaseMongoDocument, System.IEquatable<Tag>
    {                
        public string TagValue { get; set; }

        public string TagText { get; set; }

        public Tag()
        {
            
        }

        public Tag(string pTagValue, string pTagText)
        {
            Id = ObjectId.GenerateNewId().ToString();
            CreatedDate = DateTimeOffset.UtcNow;
            DeletedFlag = false;
            TagValue = pTagValue;
            TagText = pTagText;
        }

        public bool Equals(Tag otherTag)
        {
            if (otherTag.Id == this.Id)
            {
                return true;
            }
            else return false;
        }
    }
}