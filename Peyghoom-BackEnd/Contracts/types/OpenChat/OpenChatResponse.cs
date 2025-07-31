using Peyghoom_BackEnd.Entities;

namespace Peyghoom_BackEnd.Contracts.types
{
    public class OpenChatResponse
    {
        public required string UserId { get; set; }
        public List<Messages> Messages { get; set; } = new();
    }
}
