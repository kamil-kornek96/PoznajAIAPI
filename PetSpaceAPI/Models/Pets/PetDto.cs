namespace PetSpaceAPI.Models.Pets
{
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public DateTime BirthDate { get; set; }
        public string OwnerName { get; set; }
    }
}
