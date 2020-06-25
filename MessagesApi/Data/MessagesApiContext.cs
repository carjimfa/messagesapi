using System.Collections.Generic;
using MessagesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace MessagesApi.Data
{
    public class MessagesApiContext
    {
        public MessagesApiContext()
        {
            Messages=new List<Message>();
            Users=new List<User>();
        }

        public List<Message> Messages { get; set; }
        public List<User> Users { get; set; }

    }
}