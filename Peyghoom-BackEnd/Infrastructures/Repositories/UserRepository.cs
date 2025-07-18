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

        public async Task<List<Users>> GetAllUsersAsync()
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

        public async Task<bool> DoesUserExistAsync(Users user)
        {
            var database = _context.GetRPeyghoomDatabase();
            try
            {
                var findedUser = await database.GetCollection<Users>("users").Find(user => user.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();
                if (findedUser == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
            catch (Exception exception)
            {
                throw;
            }
        }


        public async Task<string> InsertUserAsync(Users user)
        {
            var database = _context.GetRPeyghoomDatabase();
            try
            {
                await database.GetCollection<Users>("users").InsertOneAsync(user);

                return user.Id.ToString();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

    }
}
