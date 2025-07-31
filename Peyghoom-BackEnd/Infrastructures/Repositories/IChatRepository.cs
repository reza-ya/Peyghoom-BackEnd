
using Peyghoom_BackEnd.Contracts.HubTypes;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public interface IChatRepository
    {
        Task SendMessageAsync(string senderUserName, string receiverUserName, string message, bool received);
        Task<List<GetOffSetMessagesResponse>> GetOffSetMessagesAsync(string userId);
    }
}