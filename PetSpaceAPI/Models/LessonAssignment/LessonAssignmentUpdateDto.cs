namespace PoznajAI.Models.LessonAssignment
{
    public class LessonAssignmentUpdateDto
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
