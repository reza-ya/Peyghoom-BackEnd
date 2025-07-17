using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Peyghoom_BackEnd.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Created_At = DateTime.Now;
        }
        [BsonId] // Marks this as the primary key (_id)
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("created_at")]
        public DateTime Created_At { get; set; }
    }
}
