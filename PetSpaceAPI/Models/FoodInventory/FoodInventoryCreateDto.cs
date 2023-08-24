using System.ComponentModel.DataAnnotations;

namespace PetSpaceAPI.Models.FoodInventory
{
    public class FoodInventoryCreateDto
    {
        [Required]
        public string FoodName { get; set; }

        [Required]
        public double QuantityInGrams { get; set; }

    }
}
