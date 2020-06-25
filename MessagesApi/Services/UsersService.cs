using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagesApi.Data;
using MessagesApi.Domain;
using MessagesApi.Dtos;
using MessagesApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MessagesApi.Services
{
    public class UsersService:IUsersService
    {
        private readonly MessagesApiContext _context;

        public UsersService(MessagesApiContext context)
        {
            _context = context;
        }
        
        public async Task<User> Login(LoginRequestDto dto)
        {
            if (await UserExists(dto.Username))
            {
                return (await GetAllActive()).FirstOrDefault(u => u.Username.ToUpper() == dto.Username.ToUpper());
            }

            return await CreateUser(dto.Username);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return (await GetAllActive()).ToList();
        }

        public async Task<bool> UserExists(Guid userId)
        {
            return (await GetAllActive()).Any(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllActive()
        {
            return _context.Users.Where(u => !u.IsDeleted);
        }

        public async Task<bool> UserExists(string username)
        {
            return (await GetAllActive()).Any(u => u.Username.ToUpper() == username.ToUpper());
        }

        private async Task<User> CreateUser(string username)
        {
            var newUser = new User()
            {
                Username = username
            };
            _context.Users.Add(newUser);
            return newUser;
        }
    }
}