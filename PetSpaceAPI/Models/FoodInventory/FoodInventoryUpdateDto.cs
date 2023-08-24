using System.ComponentModel.DataAnnotations;

namespace PetSpaceAPI.Models.FoodInventory
{
    public class FoodInventoryUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FoodName { get; set; }

        [Required]
        public double QuantityInGrams { get; set; }

    }
}
