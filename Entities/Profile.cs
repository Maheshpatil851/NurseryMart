using MongoDB.Bson.Serialization.Attributes;

namespace NurseryMart.Entities
{
    public class Profile
    {
        [BsonElement("first_name")]
        public required string FirstName { set; get; }
        [BsonElement("last_name")]
        public string? LastName { set; get; }
        [BsonElement("gender")]
        public char Gender { set; get; }
        [BsonElement("dp_url")]
        public string? DpUrl { set; get; }
        [BsonElement("dob")]
        public DateTime? DateOfBirth { set; get; }
        [BsonElement("referral_code")]
        public string? ReferralCode { set; get; }
    }
}
