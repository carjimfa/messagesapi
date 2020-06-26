using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MessagesApi.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        [MinLength(6)]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
}