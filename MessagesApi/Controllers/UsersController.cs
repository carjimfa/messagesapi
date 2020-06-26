using System.Threading.Tasks;
using MessagesApi.Dtos;
using MessagesApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessagesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase
    {
        private readonly IUsersService _usersService;
        
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _usersService.Login(dto));
            }

            return BadRequest(ModelState);
        }

    }
}