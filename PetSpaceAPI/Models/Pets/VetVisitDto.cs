namespace PetSpaceAPI.Models.Pets
{
    public class VetVisitDto
    {
        public int Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string VetName { get; set; }
        public string Diagnosis { get; set; }
        public int PetId { get; set; }
    }
}
