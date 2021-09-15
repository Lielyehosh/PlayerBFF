using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Utils.Interfaces
{
    public interface IDbModel
    {
        /// <summary>
        /// The object id in the database
        /// </summary>
        [BsonId]
        [BsonIgnoreIfDefault]
        string Id { get; set; }

        /// <summary>
        /// The creation time of this object (UTC) 
        /// </summary>
        DateTime CreateAt { get; set; }
        
        /// <summary>
        /// The modification time of this object (UTC) 
        /// </summary>
        DateTime ModifyAt { get; set; }
    }
}