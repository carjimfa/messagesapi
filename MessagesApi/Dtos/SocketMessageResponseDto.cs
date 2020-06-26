using Newtonsoft.Json;

namespace MessagesApi.Dtos
{
    public class SocketMessageResponseDto<T>
    {
        [JsonProperty(PropertyName = "type")]
        public MessageType Type { get; set; }
        [JsonProperty(PropertyName = "content")]
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