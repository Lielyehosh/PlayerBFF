using Common.Utils.DbModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Models.DbModels
{
    public class Settings : DbModel
    {
        [BsonRepresentation(BsonType.String)]
        public string SiteName { get; set; }
    }
}