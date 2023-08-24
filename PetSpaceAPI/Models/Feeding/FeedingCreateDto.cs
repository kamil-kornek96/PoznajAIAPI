using System.ComponentModel.DataAnnotations;

namespace PetSpaceAPI.Models.Feeding
{
    public class FeedingCreateDto
    {
        [Required]
        public DateTime FeedingTime { get; set; }

        [Required]
        public double AmountInGrams { get; set; }

        [Required]
        public int PetId { get; set; }
    }
}
