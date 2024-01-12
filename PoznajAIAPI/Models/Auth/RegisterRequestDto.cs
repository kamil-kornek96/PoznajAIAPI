using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.Auth
{
    public class RegisterRequestDto
    {

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

    }

}
