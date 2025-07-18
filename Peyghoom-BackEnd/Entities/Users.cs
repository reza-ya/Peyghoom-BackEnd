using MongoDB.Bson.Serialization.Attributes;

namespace Peyghoom_BackEnd.Entities
{
    public class Users : BaseEntity
    {
        [BsonElement("username")]
        public required string Username { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("phone-number")]
        public required long PhoneNumber { get; set; }
        [BsonElement("first-name")]
        public required string FirstName { get; set; }
        [BsonElement("last-name")]
        public required string LastName { get; set; }
    }
}
