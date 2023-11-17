namespace PoznajAI.Services
{
    public class UpdateLessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Duration { get; set; }
        public string Video { get; set; }
        public bool IsGptActive { get; set; }

        public Guid CourseId { get; set; }
    }
}