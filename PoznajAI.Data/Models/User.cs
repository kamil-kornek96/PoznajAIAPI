using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] PasswordSalt { get; set; }
        [MaxLength(100)]
        public string EmailConfirmationToken { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Role> Roles { get; set; }

        public ICollection<Course> Courses { get; set; }
    }



}


