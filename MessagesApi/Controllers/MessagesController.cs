using System.Threading.Tasks;
using MessagesApi.Dtos;
using MessagesApi.Exceptions;
using MessagesApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessagesApi.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController:ControllerBase
    {
        private readonly IMessagesService _messagesService;
        
        public MessagesController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostMessageRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _messagesService.PostNewMessage(dto));
                }
                catch (NotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return BadRequest(ModelState);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _messagesService.GetAll());
        }
        
    }
}