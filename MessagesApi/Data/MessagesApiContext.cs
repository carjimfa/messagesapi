using System.Collections.Generic;
using MessagesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace MessagesApi.Data
{
    public class MessagesApiContext
    {
        public MessagesApiContext()
        {
            //TODO Add initial user and messages
            Users=new List<User>();
            Messages = new List<Message>();
        }

        public List<Message> Messages { get; set; }
        public List<User> Users { get; set; }

    }
}