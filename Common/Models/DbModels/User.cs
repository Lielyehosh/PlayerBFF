using Common.Utils.DbModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Models.DbModels
{
    public class User : DbModel
    {
        [BsonRepresentation(BsonType.String)]
        public string IdNumber { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string Username { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string EmailAddress { get; set; }
    }
}