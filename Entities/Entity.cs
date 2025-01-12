using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NurseryMart.Entities
{
    public class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { set; get; }
        
    }
}
