using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagesApi.Data;
using MessagesApi.Domain;
using MessagesApi.Dtos;
using MessagesApi.Exceptions;
using MessagesApi.Services.Interfaces;

namespace MessagesApi.Services
{
    public class MessagesService:IMessagesService
    {
        private readonly MessagesApiContext _context;
        private readonly IUsersService _usersService;
        
        public MessagesService(MessagesApiContext context,
            IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }
        
        public async Task<Message> PostNewMessage(PostMessageRequestDto dto)
        {
            if (await _usersService.UserExists(dto.UserId))
            {
                var newMessage = new Message()
                {
                    Content = dto.Content,
                    UserId = dto.UserId
                };
                _context.Messages.Add(newMessage);
                await _context.SaveChangesAsync();
                return newMessage;
            }
            throw new NotFoundException("User with that id not found");
            
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            return (await GetActiveMessages()).ToList();
        }

        private async Task<IEnumerable<Message>> GetActiveMessages()
        {
            return _context.Messages.Where(m => !m.IsDeleted);
        }
    }
}