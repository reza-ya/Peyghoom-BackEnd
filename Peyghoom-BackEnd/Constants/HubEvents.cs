namespace Peyghoom_BackEnd.Constants
{
    public static class HubEvents
    {
        public static readonly string ClientsUpdated = nameof(ClientsUpdated);
        public static readonly string ReceiveMessage = nameof(ReceiveMessage);
        public static readonly string ReceiveRegister = nameof(ReceiveRegister);
        public static readonly string GetChats = nameof(GetChats);
    }
}
