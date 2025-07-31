using MongoDB.Bson;
using Peyghoom_BackEnd.Entities;

namespace Peyghoom_BackEnd.Contracts.HubTypes
{
    public class GetOffSetMessagesResponse
    {
        public ObjectId UserId { get; set; }
        public int UnReadCount { get; set; }
        public List<Messages> Messages { get; set; } = new();
    }
}
