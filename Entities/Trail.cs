using MongoDB.Bson.Serialization.Attributes;

namespace NurseryMart.Entities
{
    public class Trail
    {
        [BsonElement("created_on")]
        public DateTimeOffset? CreatedOn { set; get; }
        [BsonElement("modified_on")]
        public DateTimeOffset? ModifiedOn { set; get; }
        [BsonElement("created_by")]
        public string? CreatedBy { set; get; }
        [BsonElement("modified_by")]
        public string? ModifiedBy { set; get; }
        [BsonElement("is_active")]
        public bool IsActive { set; get; }
        [BsonElement("is_marked_as_delete")]
        public bool IsMarkedAsDelete { set; get; }
        [BsonElement("last_activity_on")]
        public DateTimeOffset? LastActiveOn { set; get; }
    }
}
