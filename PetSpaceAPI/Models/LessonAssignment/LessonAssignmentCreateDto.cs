namespace PoznajAI.Models.LessonAssignment
{
    public class LessonAssignmentCreateDto
    {

        public int LessonId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
