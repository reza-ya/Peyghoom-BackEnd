
namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public interface IChatRepository
    {
        Task SendMessageAsync(string senderUserName, string receiverUserName, string message);
    }
}