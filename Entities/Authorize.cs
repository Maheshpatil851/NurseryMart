using MongoDB.Bson.Serialization.Attributes;
using NurseryMart.IRepository;

namespace NurseryMart.Entities
{
    public class Authorize : IEntity
    {

        [BsonElement("mobile")]
        public string Mobile { set; get; }
        [BsonElement("profile")]
        public Profile Profile { set; get; }
        [BsonElement("email")]
        public string Email { set; get; }
        [BsonElement("password")]
        public string Password { set; get; }
        [BsonElement("role")]
        public string Role { set; get; }
        [BsonElement("address")]
        public string Address { set; get; }
        [BsonElement("country")]
        public string Country { set; get; }
        [BsonElement("state")]
        public string State { set; get; }
        [BsonElement("city")]
        public string City { set; get; }
        [BsonElement("pincode")]
        public string Pincode { set; get; }
        [BsonElement("trail")]
        public required Trail Trail { set; get; }

    }
}
