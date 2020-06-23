using System;

namespace MessagesApi.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message)
        {
            
        }
    }
}