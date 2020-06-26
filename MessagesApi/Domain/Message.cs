using System;
using Newtonsoft.Json;

namespace MessagesApi.Domain
{
    public class Message:BaseEntity
    {
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}