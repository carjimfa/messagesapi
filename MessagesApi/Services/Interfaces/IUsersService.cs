using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagesApi.Domain;
using MessagesApi.Dtos;

namespace MessagesApi.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> Login(LoginRequestDto dto);
        Task<IEnumerable<User>> GetAll();
        Task<bool> UserExists(Guid userId);
    }
}