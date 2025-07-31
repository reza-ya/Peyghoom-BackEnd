using Peyghoom_BackEnd.Contracts.types;
using Peyghoom_BackEnd.Entities;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllUsersAsync();
        Task<string> InsertUserAsync(Users user);
        Task<bool> DoesUserExistAsync(Users user);
        Task<OpenChatResponse> OpenChatAsync(string user, string username);
    }
}