using Peyghoom_BackEnd.Entities;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllUsersAsync();
        Task<string> InsertUserAsync(Users user);
        Task<bool> DoesUserExistAsync(Users user);
        Task<List<Messages>> OpenChatAsync(string user, string username);
    }
}