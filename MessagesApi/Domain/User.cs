using System.Collections.Generic;

namespace MessagesApi.Domain
{
    public class User:BaseEntity
    {
        public string Username { get; set; }
        public List<Message> Messages { get; set; }
    }
}