using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }

        public int ModuleId { get; set; }
        public CourseModule Module { get; set; }

        public ICollection<LessonAssignment> Assignments { get; set; } = new List<LessonAssignment>();
        public ICollection<LessonComment> Comments { get; set; } = new List<LessonComment>();
        public ICollection<LessonRating> Ratings { get; set; } = new List<LessonRating>();
    }
}
