namespace PetSpace.Data.Models
{
    public class FoodConsumption
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double AmountInGrams { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
