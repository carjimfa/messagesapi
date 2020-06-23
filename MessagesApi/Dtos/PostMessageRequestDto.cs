using System;
using System.ComponentModel.DataAnnotations;

namespace MessagesApi.Dtos
{
    public class PostMessageRequestDto
    {
        [MinLength(1)]
        [MaxLength(140)]
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }
}