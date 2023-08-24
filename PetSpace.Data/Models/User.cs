using System.ComponentModel.DataAnnotations;

namespace PetSpace.Data.Models
{
    public class User
    {
        public int Id { get; set; }

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
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] PasswordHash { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Role Role { get; set; }

        public ICollection<Pet> Pets { get; set; }

        public ICollection<FoodInventory> FoodInventories { get; set; }
    }



}


