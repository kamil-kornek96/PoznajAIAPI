﻿namespace PoznajAI.Models.Course
{
    public class UserCoursesResponseDto
    {
        public IEnumerable<OwnedCourseDto> OwnedCourses { get; set; }
        public IEnumerable<CourseDto> AllCourses { get; set; }
    }
}