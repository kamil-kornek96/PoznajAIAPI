using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Models
{
    public class LessonComment
    {
        public int Id { get; set; }

        [Required]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
