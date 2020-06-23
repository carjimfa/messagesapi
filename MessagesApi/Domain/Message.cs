using System;

namespace MessagesApi.Domain
{
    public class Message:BaseEntity
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}