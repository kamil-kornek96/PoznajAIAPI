using PoznajAI.Data.Models;

namespace PoznajAI.Services
{
    public class UserCoursesResponseDto
    {
        public IEnumerable<OwnedCourseDto> OwnedCourses { get; set; }
        public IEnumerable<CourseDto> AllCourses { get; set; }
    }
}