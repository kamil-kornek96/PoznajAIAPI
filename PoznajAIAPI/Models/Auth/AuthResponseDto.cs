using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.Auth
{
    public class TokenResponseDto
    {
        [Required]
        public string Token { get; set; }
    }
}
