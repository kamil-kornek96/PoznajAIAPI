using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Models
{
    public class CompletedLesson
    {
        public int Id { get; set; }

        public int CourseUserId { get; set; }
        public CourseUser CourseUser { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }

}
