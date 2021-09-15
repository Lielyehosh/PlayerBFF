using System;
using Common.Utils.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Utils.DbModels
{
    public class DbModel : IDbModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreateAt { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModifyAt { get; set; } = DateTime.UtcNow;
    }
}