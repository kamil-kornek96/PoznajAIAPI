namespace PoznajAI.Models.LessonComment
{
    public class LessonCommentCreateDto
    {

        public int LessonId { get; set; }

        public int UserId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
