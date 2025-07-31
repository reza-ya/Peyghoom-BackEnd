using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Peyghoom_BackEnd.AAA;
using Peyghoom_BackEnd.Constants;
using Peyghoom_BackEnd.Contracts.HubTypes;
using Peyghoom_BackEnd.Infrastructures.Repositories;

namespace Peyghoom_BackEnd.Hubs
{

    // TODO: authorize by policy
    //[Authorize(MyAuthorizationPolicy.RegisteredPolicy)]
    [Authorize]
    public class ChatHub : Hub
    {
        private IUserRepository _userRepository;
        private IChatRepository _chatRepository;
        private static List<OnlineUser> _onlineUsers = new List<OnlineUser>();


        public ChatHub(IUserRepository userRepository, IChatRepository chatRepository)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
        }
        public async Task SendMessage(string message, string userId)
        {
            var senderUsername = Context?.User?.Claims.FirstOrDefault(claim => claim.Type == MyClaimTypes.UserName)?.Value;
            var senderUserId = Context?.User?.Claims.FirstOrDefault(claim => claim.Type == MyClaimTypes.SubId)?.Value;

            if (senderUsername == null || senderUserId == null)
            {
                //TODO: 
                throw new Exception();
            }


            var onlineUser = _onlineUsers.FirstOrDefault(User => User.UserName == userId);
            if (onlineUser != null)
            {
                var chosenClient = Clients.Client(onlineUser.ConnectionId);
                await chosenClient.SendAsync(HubEvents.ReceiveMessage, message);
                await _chatRepository.SendMessageAsync(senderUserId, userId, message, true);
            }
            else
            {

                await _chatRepository.SendMessageAsync(senderUserId, userId, message, false);
            }
        }


        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var test = _userRepository.GetAllUsersAsync();
            var username = Context?.User?.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.UserName)?.Value;
            var userId = Context?.User?.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.SubId)?.Value;
            if (userId == null || username == null)
            {
                //TODO: 
                throw new Exception();
            }
            var onlineUser = _onlineUsers.FirstOrDefault(User => User.UserName == username);
            if (onlineUser != null)
            {
                onlineUser.ConnectionId = connectionId;
            }
            else
            {
                _onlineUsers.Add(new OnlineUser { UserName = username, ConnectionId = connectionId, UserId = userId });
                onlineUser = _onlineUsers.FirstOrDefault(User => User.UserName == username);
            }
            //await Clients.All.SendAsync(HubEvents.ClientsUpdated, _onlineUsers);
            await Clients.Client(connectionId).SendAsync(HubEvents.ReceiveRegister, onlineUser);

            var getOffSetMessagesResponses = await _chatRepository.GetOffSetMessagesAsync(userId);
            await Clients.Clients(connectionId).SendAsync(HubEvents.GetChats, getOffSetMessagesResponses);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            var username = Context?.User?.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.UserName)?.Value;

            var onlineUser = _onlineUsers.RemoveAll(User => User.UserName == username);


            await Clients.All.SendAsync(HubEvents.ClientsUpdated, _onlineUsers);
            await base.OnDisconnectedAsync(exception);
        }
    }
}

public class OnlineUser
{
    public string UserName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
}
