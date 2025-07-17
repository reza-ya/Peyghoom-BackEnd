using Peyghoom_BackEnd.Entities;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAllUsers();
    }
}