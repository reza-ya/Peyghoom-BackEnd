using MongoDB.Bson;
using MongoDB.Driver;
using Peyghoom_BackEnd.Entities;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public class UserRepository : IUserRepository
    {
        IPeyghoomContext _context;
        public UserRepository(IPeyghoomContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            var database = _context.GetRPeyghoomDatabase();
            try
            {
                var users = await database.GetCollection<Users>("users").Find(new BsonDocument()).ToListAsync();

                if (users == null)
                {
                    throw new Exception("users does not exist");
                }
                return users;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

    }
}
