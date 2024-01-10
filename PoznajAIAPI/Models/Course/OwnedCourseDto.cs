using PoznajAI.Models.Lesson;

namespace PoznajAI.Models.Course
{
    public class OwnedCourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<LessonDetailsDto> Lessons { get; set; }
    }
}