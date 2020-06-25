namespace MessagesApi.Dtos
{
    public class SocketMessageResponseDto<T>
    {
        public MessageType Type { get; set; }
        public T Content { get; set; }
    }

    public enum MessageType
    {
        UserConnected=0,
        NewMessage=1,
        AllMessages=2,
        SocketUserInfo=3
    }
}