namespace PetSpaceAPI.Models.FoodConsumption
{
    public class FoodConsumptionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double AmountInGrams { get; set; }
        public int PetId { get; set; }
    }
}
