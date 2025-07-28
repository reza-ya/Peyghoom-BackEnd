using MongoDB.Bson;
using MongoDB.Driver;
using Peyghoom_BackEnd.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Peyghoom_BackEnd.Infrastructures.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private IPeyghoomContext _context;

        public ChatRepository(IPeyghoomContext context)
        {
            _context = context;
        }
        public async Task SendMessageAsync(string senderUserId, string receiverUserName, string message)
        {


            var database = _context.GetRPeyghoomDatabase();
            var requestedUser = await database.GetCollection<Users>("users").Find(u => u.Username == receiverUserName).FirstOrDefaultAsync();

            var newMessage = new Messages
            {
                CreateAt = DateTime.UtcNow,
                ReceiverUserId = requestedUser.Id,
                SenderUserId = ObjectId.Parse(senderUserId),
                Text = "Hello",
                Received = false
            };


            var filter = Builders<Users>.Filter.And(
                            Builders<Users>.Filter.Eq(u => u.Id, new ObjectId(senderUserId)),
                            Builders<Users>.Filter.ElemMatch(u => u.Chats, c => c.UserId == requestedUser.Id)
);

            var update = Builders<Users>.Update.Push("chats.$.messages", message);

            var result = await database.GetCollection<Users>("users").UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                // Optionally: handle case where chat doesn’t exist yet
                throw new Exception("Chat not found for this user.");
            }
            await Task.CompletedTask;
        }
    }
}
