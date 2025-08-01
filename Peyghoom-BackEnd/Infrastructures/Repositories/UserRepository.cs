using MongoDB.Bson;
using MongoDB.Driver;
using Peyghoom_BackEnd.Entities;
using Peyghoom_BackEnd.Contracts.types;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public class UserRepository : IUserRepository
    {
        IPeyghoomContext _context;
        private ILogger _logger;

        public UserRepository(IPeyghoomContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                //TODO: error handling
                _logger.LogError(exception.Message);
                throw;
            }
        }

        public async Task<bool> DoesUserExistAsync(Users userToCheck)
        {
            var database = _context.GetRPeyghoomDatabase();
            try
            {
                var findedUser = await database.GetCollection<Users>("users").Find(user => user.PhoneNumber == userToCheck.PhoneNumber).FirstOrDefaultAsync();
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
                _logger.LogError(exception.Message);
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
                _logger.LogError(exception.Message);
                throw;
            }
        }


        public async Task<OpenChatResponse> OpenChatAsync(string userId, string username)
        {
            var messages = new List<Messages>();
            var database = _context.GetRPeyghoomDatabase();
            
            var findedUser = await database.GetCollection<Users>("users").Find(u => u.Id == new ObjectId(userId)).FirstOrDefaultAsync();
            var requestedUser = await database.GetCollection<Users>("users").Find(u => u.Username == username).FirstOrDefaultAsync();
            if (findedUser.Chats != null)
            {
                var chat = findedUser.Chats.Where(chat => chat.UserId == requestedUser.Id).FirstOrDefault();
                if (chat != null)
                {
                    var findedMessage = chat.Messages.OrderByDescending(m => m.CreateAt).Take(10).ToList();
                    messages.AddRange(findedMessage);
                }
            }
            else
            {
                var newChat = new Chats() { CreateAt = DateTime.Now, Messages = new List<Messages>(), UserId = requestedUser.Id };

                var update = Builders<Users>.Update.Push(u => u.Chats, newChat);
                var result =  database.GetCollection<Users>("users").UpdateOneAsync(
                    u => u.Id == ObjectId.Parse(userId),
                    update
                );
            }
            return new OpenChatResponse() { UserId = requestedUser.Id.ToString() , Messages = messages };

        }
    }
}
