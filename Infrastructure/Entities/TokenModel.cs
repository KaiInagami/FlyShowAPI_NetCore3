using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities
{
    public class TokenModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }
        [BsonElement("Token")]
        public string Token { get; set; }

        [BsonElement("Account")]
        public string Account { get; set; }

        [BsonElement("CurrentTime")]
        public string CurrentTime { get; set; }

        [BsonElement("ExpireTime")]
        public string ExpireTime { get; set; }

        [BsonElement("IsValid")]
        public bool IsValid { get; set; }
    }
}
