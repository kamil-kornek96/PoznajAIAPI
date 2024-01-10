namespace PoznajAI.Models.Lesson
{
    public class LessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public Guid CourseId { get; set; }

    }
}