using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Models
{
    public class LessonRating
    {
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
