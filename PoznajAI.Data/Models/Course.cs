namespace PoznajAI.Data.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Lesson> Lessons { get; set; }

        public ICollection<User> Users { get; set; }
    }



}

