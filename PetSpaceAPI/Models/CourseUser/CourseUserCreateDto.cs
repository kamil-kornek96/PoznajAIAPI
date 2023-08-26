﻿namespace PoznajAI.Models.CourseUser
{
    public class CourseUserCreateDto
    {

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public DateTime EnrolledDate { get; set; }

        public List<int> CompletedLessons { get; set; }
    }
}
