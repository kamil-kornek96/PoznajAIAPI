namespace PetSpaceAPI.Models.Feeding
{
    public class FeedingDto
    {
        public int Id { get; set; }
        public DateTime FeedingTime { get; set; }
        public double AmountInGrams { get; set; }
        public int PetId { get; set; }
    }
}
