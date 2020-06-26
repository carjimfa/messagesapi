using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MessagesApi.Dtos
{
    public class PostMessageRequestDto
    {
        [MinLength(1)]
        [MaxLength(140)]
        [Required]
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [Required]
        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }
    }
}