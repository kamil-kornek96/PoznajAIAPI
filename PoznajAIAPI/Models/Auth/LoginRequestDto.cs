using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
