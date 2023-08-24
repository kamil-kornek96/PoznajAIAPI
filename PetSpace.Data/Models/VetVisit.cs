namespace PetSpace.Data.Models
{
    public class VetVisit
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string VetName { get; set; }
        public string Diagnosis { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
