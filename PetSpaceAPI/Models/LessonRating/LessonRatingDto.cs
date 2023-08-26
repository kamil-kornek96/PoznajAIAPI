using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.LessonRating
{
    public class LessonRatingDto
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
