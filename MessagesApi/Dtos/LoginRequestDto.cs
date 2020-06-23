using System.ComponentModel.DataAnnotations;

namespace MessagesApi.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
    }
}