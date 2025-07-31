using MongoDB.Bson;
using MongoDB.Driver;
using Peyghoom_BackEnd.Contracts.HubTypes;
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
        public async Task SendMessageAsync(string senderUserId, string receiverUserId, string message, bool received)
        {


            var database = _context.GetRPeyghoomDatabase();

            var newMessage = new Messages
            {
                CreateAt = DateTime.UtcNow,
                ReceiverUserId = ObjectId.Parse(receiverUserId),
                SenderUserId = ObjectId.Parse(senderUserId),
                Text = message,
                Received = received
            };

            var recieverFilter = Builders<Users>.Filter.And(
                            Builders<Users>.Filter.Eq(u => u.Id, new ObjectId(receiverUserId)),
                            Builders<Users>.Filter.ElemMatch(u => u.Chats, c => c.UserId == ObjectId.Parse(senderUserId)));

            var senderFilter = Builders<Users>.Filter.And(
                            Builders<Users>.Filter.Eq(u => u.Id, new ObjectId(senderUserId)),
                            Builders<Users>.Filter.ElemMatch(u => u.Chats, c => c.UserId == ObjectId.Parse(receiverUserId)));

            var update = Builders<Users>.Update.Push("chats.$.messages", newMessage);

            var senderResult = await database.GetCollection<Users>("users").UpdateOneAsync(senderFilter, update);
            var receiverResult = await database.GetCollection<Users>("users").UpdateOneAsync(recieverFilter, update);

            if (senderResult.MatchedCount == 0 || receiverResult.MatchedCount == 0)
            {
                // Optionally: handle case where chat doesn’t exist yet
                throw new Exception("Chat not found for this user.");
            }
            await Task.CompletedTask;
        }

        public async Task<List<GetOffSetMessagesResponse>> GetOffSetMessagesAsync(string userId)
        {
            var result = new List<GetOffSetMessagesResponse>();


            var usersCollection = _context
                                .GetRPeyghoomDatabase()
                                .GetCollection<Users>("users");

            var userIdObj = new ObjectId(userId);

            var userWithChats = await usersCollection
                .Find(u => u.Id == userIdObj)
                .Project(u => new
                {
                    u.Username,
                    u.FirstName,
                    u.LastName,
                    UserId = u.Id,
                    TopChats = u.Chats
                        .OrderByDescending(c => c.CreateAt)
                        .Take(10)
                        .Select(c => new
                        {
                            c.UserId,
                            c.CreateAt,
                            UnreadCount = c.Messages.Count(m => m.Received == false),
                            TopMessages = c.Messages
                                .OrderByDescending(m => m.CreateAt)
                                .Take(5)
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            foreach(var chat in userWithChats.TopChats)
            {
                var getOffSetMessagesResponse = new GetOffSetMessagesResponse()
                {
                    UserId = chat.UserId,
                    Messages = chat.TopMessages.ToList(),
                    UnReadCount = chat.UnreadCount,
                };

                result.Add(getOffSetMessagesResponse);
            }

            return result;
        }
    }
}
