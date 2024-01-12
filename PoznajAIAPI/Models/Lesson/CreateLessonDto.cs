using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.Lesson
{
    public class CreateLessonDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Duration { get; set; }

        public string Video { get; set; }
        public bool IsGptActive { get; set; }

        public Guid CourseId { get; set; }
    }
}