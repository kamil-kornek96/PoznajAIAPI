using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.LessonComment
{
    public class LessonCommentDto
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int UserId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
