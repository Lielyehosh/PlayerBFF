using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.DbModels
{
    public class User
    {
        [BsonElement]
        public string IdNumber { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string EmailAddress { get; set; }
    }
}