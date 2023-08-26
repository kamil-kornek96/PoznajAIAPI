using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Models
{
    public class CourseUser
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int CourseModuleId { get; set; }
        public CourseModule Course { get; set; }

        [Required]
        public DateTime EnrolledDate { get; set; }

        public ICollection<CompletedLesson> CompletedLessons { get; set; } = new List<CompletedLesson>();
    }

}
