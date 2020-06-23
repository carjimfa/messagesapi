using System.Collections.Generic;
using System.Threading.Tasks;
using MessagesApi.Domain;
using MessagesApi.Dtos;

namespace MessagesApi.Services.Interfaces
{
    public interface IMessagesService
    {
        public Task<Message> PostNewMessage(PostMessageRequestDto dto);
        public Task<IEnumerable<Message>> GetAll();
    }
}