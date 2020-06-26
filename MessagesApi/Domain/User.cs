using System.Collections.Generic;
using Newtonsoft.Json;

namespace MessagesApi.Domain
{
    public class User:BaseEntity
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "messages")]
        public List<Message> Messages { get; set; }
    }
}