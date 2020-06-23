using System.Collections.Generic;
using MessagesApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace MessagesApi.Data
{
    public class MessagesApiContext:DbContext
    {
        public MessagesApiContext(DbContextOptions<MessagesApiContext> options)
            : base(options)
        {
            
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

    }
}