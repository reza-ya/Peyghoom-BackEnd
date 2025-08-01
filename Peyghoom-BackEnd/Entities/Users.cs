using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Peyghoom_BackEnd.Entities
{
    public class Users : BaseEntity
    {
        [BsonElement("username")]
        public required string Username { get; set; }
        [BsonElement("phone_number")]
        public required long PhoneNumber { get; set; }
        [BsonElement("first_name")]
        public required string FirstName { get; set; }
        [BsonElement("last_name")]
        public required string LastName { get; set; }
        [BsonElement("chats")]
        public List<Chats> Chats { get; set; } = new();

    }

    public class Chats
    {
        public Chats()
        {
            CreateAt = DateTime.Now;
        }
        [BsonRepresentation(BsonType.ObjectId)]
        public required ObjectId UserId { get; set; }
        [BsonElement("created_at")]
        public required DateTime CreateAt { get; set; }
        [BsonElement("messages")]
        public required List<Messages> Messages { get; set; }

    }

    public class Messages
    {
        public Messages()
        {
            CreateAt = DateTime.Now;
        }
        [BsonElement("created_at")]
        public required DateTime CreateAt { get; set; }
        //[BsonElement("receiver_username")]
        [BsonRepresentation(BsonType.ObjectId)]
        public required ObjectId ReceiverUserId { get; set; }
        //[BsonElement("recipient_username")]
        [BsonRepresentation(BsonType.ObjectId)]
        public required ObjectId SenderUserId { get; set; }
        [BsonElement("text")]
        public required string Text { get; set; }
        [BsonElement("received")]
        public bool Received { get; set; }
    }
}
