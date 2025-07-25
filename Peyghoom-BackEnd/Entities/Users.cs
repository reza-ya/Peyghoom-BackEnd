using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Peyghoom_BackEnd.Entities
{
    public class Users : BaseEntity
    {
        [BsonElement("username")]
        public required string Username { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("phone_number")]
        public required long PhoneNumber { get; set; }
        [BsonElement("first_name")]
        public required string FirstName { get; set; }
        [BsonElement("last_name")]
        public required string LastName { get; set; }
        [BsonElement("chats")]
        public List<Chats> Chats { get; set; }

}

    public class Chats
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public required string UserId { get; set; }
        [BsonElement("created_at")]
        public required DateTime CreateAt { get; set; }
        [BsonElement("messages")]
        public required List<Messages> Messages { get; set; }

    }

    public class Messages
    {
        [BsonElement("receiver_id")]
        public required string ReceiverId { get; set; }
        [BsonElement("recipient_id")]
        public required string RecipientId { get; set; }
        [BsonElement("text")]
        public required string Text { get; set; }
        [BsonElement("received")]
        public bool Received { get; set; }
    }
}
