using PoznajAI.Data.Models;

namespace PoznajAI.Services
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<LessonDto> Lessons { get; set; }
    }
}