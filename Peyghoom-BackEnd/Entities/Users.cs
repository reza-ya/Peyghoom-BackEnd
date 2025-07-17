using MongoDB.Bson.Serialization.Attributes;

namespace Peyghoom_BackEnd.Entities
{
    public class Users : BaseEntity
    {
        [BsonElement("username")]
        public required string Username { get; set; }
        [BsonElement("password")]
        public required string Password { get; set; }
    }
}
