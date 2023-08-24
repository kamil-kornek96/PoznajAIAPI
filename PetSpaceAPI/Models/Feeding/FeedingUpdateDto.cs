using System.ComponentModel.DataAnnotations;

namespace PetSpaceAPI.Models.Feeding
{
    public class FeedingUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime FeedingTime { get; set; }

        [Required]
        public double AmountInGrams { get; set; }
    }
}
