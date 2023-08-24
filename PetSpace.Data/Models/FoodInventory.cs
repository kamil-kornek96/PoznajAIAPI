namespace PetSpace.Data.Models
{
    public class FoodInventory
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public double QuantityInGrams { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
