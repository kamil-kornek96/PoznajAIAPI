namespace PoznajAI.Models.LessonRating
{
    public class LessonRatingCreateDto
    {

        public int LessonId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
