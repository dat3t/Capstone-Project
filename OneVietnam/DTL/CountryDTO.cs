using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OneVietnam.DTL
{
    public class Country
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public Country(string name, string code)
        {
            Id = ObjectId.GenerateNewId().ToString();
            CountryName = name;
            CountryCode = code;
        }
    }
}